using Microsoft.EntityFrameworkCore;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Repository.Context;
using ShopBackEnd.Validators.ResponseValidator;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ShopDbContext _context;

        public CategoryRepository(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Select(product => new ProductDto
                {
                    Id = product.Id,
                    ProductType = product.ProductType,
                    Name = product.Name,
                    Image = product.Image,
                    AdditionalValue = product.AdditionalValue,
                    GoldWeightInGrams = product.GoldWeightInGrams, 
                    SellingPrice = product.SellingPrice,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                    StockQuantity = product.StockQuantity, 
                    ProductState = product.ProductState,
                    LastModifiedDate = product.LastModifiedDate
                }).ToList(),
                LastModifiedDate = category.LastModifiedDate
            };
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Products = c.Products.Select(product => new ProductDto
                {
                    Id = product.Id,
                    ProductType = product.ProductType, 
                    Name = product.Name,
                    Image = product.Image,
                    AdditionalValue = product.AdditionalValue, 
                    GoldWeightInGrams = product.GoldWeightInGrams, 
                    SellingPrice = product.SellingPrice,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                    StockQuantity = product.StockQuantity, 
                    ProductState = product.ProductState,
                    LastModifiedDate = product.LastModifiedDate
                }).ToList(),
                LastModifiedDate = c.LastModifiedDate
            }).ToList();
        }
        public async Task<List<CategoryNameDto>> GetAllCategoryNamesAsync()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryNameDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    LastModifiedDate = c.LastModifiedDate
                })
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDtoAdd categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                LastModifiedDate = categoryDto.LastModifiedDate
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Return mapped CategoryDto
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                LastModifiedDate = category.LastModifiedDate
            };

        }

        public async Task<CategoryDto> EditCategoryNameAsync(int categoryId, CategoryDtoEdit categoryDtoEdit)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return null;

            category.Name = categoryDtoEdit.Name;
            category.LastModifiedDate = categoryDtoEdit.LastModifiedDate;

            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                LastModifiedDate = category.LastModifiedDate
            };
        }
        public async Task<string> GetCategoryNameByIdAsync(int id)
        {
            var name = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            return name;
        }




        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
                return false;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var product in category.Products)
                    {
                        _context.Products.Remove(product);
                    }

                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}
