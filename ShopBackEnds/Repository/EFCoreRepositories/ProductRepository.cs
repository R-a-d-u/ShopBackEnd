using Microsoft.EntityFrameworkCore;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.Data.Mapper.ProductMapper;
using ShopBackEnd.HelperClass;
using ShopBackEnd.Repository.Context;
using ShopBackEnd.Service;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IGoldHistoryRepository _goldHistoryRepository;

        public ProductRepository(ShopDbContext context, ILogger<UserService> logger, IGoldHistoryRepository goldHistoryRepository)
        {
            _context = context;
            _logger = logger;
            _goldHistoryRepository = goldHistoryRepository;
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new InvalidOperationException("Product id not found.");
            }

            return ProductMapper.ToDto(product);
        }

        public async Task<PagedResult<ProductDto>> GetAllDiscontinuedProducts(int pageNumber, int pageSize)
        {
            var query = _context.Products.AsNoTracking()
                 .Where(p => p.ProductState == ProductState.Discontinued)
                 .OrderByDescending(p => p.LastModifiedDate);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductDto>(
                products.Select(ProductMapper.ToDto),
                totalCount,
                pageNumber,
                pageSize
            );
        }

        public async Task<PagedResult<ProductDto>> GetAllProductsByCategoryId(int categoryId, int pageNumber, int pageSize)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId && p.ProductState != ProductState.Discontinued)
                 .OrderBy(p => p.ProductState == ProductState.inStock ? 0 : 1)
                 .ThenBy(p => p.SellingPrice);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductDto>(
                products.Select(ProductMapper.ToDto),
                totalCount,
                pageNumber,
                pageSize
            );
        }


        public async Task<PagedResult<ProductDto>> GetAllProductsByName(string name, int pageNumber, int pageSize)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => p.Name.ToLower().Contains(name.ToLower()) &&
                            p.ProductState != ProductState.Discontinued);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductDto>(
                products.Select(ProductMapper.ToDto),
                totalCount,
                pageNumber,
                pageSize
            );
        }


        public async Task<ProductDtoEditInformation?> GetProductInformationById(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return new ProductDtoEditInformation
            {
                Name = product.Name,
                Image = product.Image,
                CategoryId = product.CategoryId,
                Description = product.Description,
                LastModifiedDate = product.LastModifiedDate
            };
        }

        public async Task<bool> SetStateToOutOfStock(int id, ProductDtoEditState dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            dto.ProductState = ProductState.outOfStock;
            dto.LastModifiedDate = DateTime.Now;
            ProductEditStateMapper.UpdateEntity(product, dto);
            product.StockQuantity = 0;

            return await SaveChangesAsync();
        }

        public async Task<bool> SetStateToDiscontinued(int id, ProductDtoEditState dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            dto.ProductState = ProductState.Discontinued;
            dto.LastModifiedDate = DateTime.Now;
            ProductEditStateMapper.UpdateEntity(product, dto);
            product.StockQuantity = 0;

            return await SaveChangesAsync();
        }

        public async Task<bool> SetStateToInStock(int id, ProductDtoEditState dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            dto.ProductState = ProductState.inStock;
            dto.LastModifiedDate = DateTime.Now;
            ProductEditStateMapper.UpdateEntity(product, dto);
            product.StockQuantity = 1;
            return await SaveChangesAsync();
        }

        public async Task<bool> EditProductStockQuantity(int id, ProductDtoEditStock dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            dto.LastModifiedDate = DateTime.Now;
            ProductEditStockMapper.UpdateEntity(product, dto);

            product.ProductState = product.StockQuantity > 0 ? ProductState.inStock : ProductState.outOfStock;

            var cartItems = await _context.CartItems
                .Where(ci => ci.ProductId == id)
                .ToListAsync();

            bool listChanged = false;

            foreach (var cartItem in cartItems)
            {
                if (cartItem.Quantity > product.StockQuantity)
                {
                    if (product.StockQuantity > 0)
                    {
                        cartItem.Quantity = product.StockQuantity;
                    }
                    else
                    {
                        _context.CartItems.Remove(cartItem);
                    }
                    listChanged = true;
                }
            }

            if (listChanged)
            {
                await _context.SaveChangesAsync();
            }

            return await SaveChangesAsync();
        }

        public async Task<bool> EditProductAdditionalPrice(int id, ProductDtoEditSellingPrice dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            decimal latestGoldPriceInGrams = await _goldHistoryRepository.GetLastPriceInGramsAsync();

            if (latestGoldPriceInGrams <= 0)
            {
                throw new InvalidOperationException("Invalid gold price.");
            }

            product.AdditionalValue = dto.AdditionalValue;
            product.SellingPrice = (product.GoldWeightInGrams * latestGoldPriceInGrams + product.AdditionalValue) * 1.4m;

            product.LastModifiedDate = DateTime.Now;

            return await SaveChangesAsync();
        }

        public async Task<bool> EditProductInformation(int id, ProductDtoEditInformation dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            dto.LastModifiedDate = DateTime.Now;

            ProductEditInformationMapper.UpdateEntity(product, dto);

            return await SaveChangesAsync();
        }

        public async Task<int> AddProduct(ProductDtoAdd dto)
        {
            decimal latestGoldPriceInGrams = await _goldHistoryRepository.GetLastPriceInGramsAsync();
            if (latestGoldPriceInGrams <= 0)
            {
                throw new InvalidOperationException("Invalid gold price.");
            }
            dto.SellingPrice = (dto.GoldWeightInGrams * latestGoldPriceInGrams + dto.AdditionalValue) * 1.4m;
            dto.LastModifiedDate = DateTime.Now;
            var product = ProductAddMapper.ToEntity(dto);
            if (product == null) return 0;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<List<ProductDto>> FilterAllProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            return products.Select(ProductMapper.ToDto).ToList();
        }

        public async Task<List<ProductDto>> FilterAllProducts(ProductDtoFilter filter)
        {
            var query = _context.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(p => p.Name.Contains(filter.Name));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.SellingPrice >= filter.MinPrice);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.SellingPrice <= filter.MaxPrice);
            }

            if (filter.ProductState.HasValue)
            {
                query = query.Where(p => p.ProductState == filter.ProductState);
            }

            var products = await query.ToListAsync();
            return products.Select(ProductMapper.ToDto).ToList();
        }
        public async Task<bool> UpdateAllGoldProductPrices()
        {
            try
            {
                decimal latestGoldPriceInGrams = await _goldHistoryRepository.GetLastPriceInGramsAsync();
                if (latestGoldPriceInGrams <= 0)
                {
                    throw new InvalidOperationException("Invalid gold price.");
                }

                var goldProducts = await _context.Products
                    .Where(p => p.ProductType == ProductType.Jewlery ||
                               p.ProductType == ProductType.GoldBars ||
                               p.ProductType == ProductType.GoldCoins)
                    .ToListAsync();

                foreach (var product in goldProducts)
                {
                    product.SellingPrice = (product.GoldWeightInGrams * latestGoldPriceInGrams + product.AdditionalValue) * 1.4m;
                    product.LastModifiedDate = DateTime.Now;
                }

                var allCarts = await _context.Carts
                    .Include(c => c.Items)
                    .ToListAsync();

                foreach (var cart in allCarts)
                {
                    _context.CartItems.RemoveRange(cart.Items);
                    cart.Items.Clear();
                }

                return await SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<int> GetTotalNonDiscontinuedProductsCount()
        {
            return await _context.Products
                .AsNoTracking()
                .CountAsync(p => p.ProductState != ProductState.Discontinued);
        }
        public async Task<int> GetTotalLowStockProductsCount()
        {
            return await _context.Products
                .AsNoTracking()
                .CountAsync(p => p.ProductState != ProductState.Discontinued &&
                                 p.StockQuantity > 0 &&
                                 p.StockQuantity < 4);
        }
    }
}