using Microsoft.EntityFrameworkCore;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Mapper.CartItemMapper;
using ShopBackEnd.Repository.Context;
using ShopBackEnd.Service;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<CartService> _logger;

        public CartRepository(ShopDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CartItemDto>> GetAllCartItems(int cartId)
        {
            bool listChanged = false;
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var itemsToRemove = new List<CartItem>();

            foreach (var item in cart.Items)
            {
                if (item.Quantity > item.Product.StockQuantity)
                {
                    if (item.Product.StockQuantity > 0)
                    {
                        item.Quantity = item.Product.StockQuantity;
                        _context.Update(item);
                        listChanged = true;
                    }
                    else
                    {
                        itemsToRemove.Add(item);
                        listChanged = true;
                    }
                }
            }

            foreach (var item in itemsToRemove)
            {
                cart.Items.Remove(item);
                _context.CartItems.Remove(item);
            }

            if (itemsToRemove.Any() || cart.Items.Any(i => i.Quantity > i.Product.StockQuantity) || listChanged == true)
            {
                await _context.SaveChangesAsync();
            }

            return cart.Items.Select(CartItemMapper.ToDto).ToList();
        }
        public async Task<int?> GetCartIdByUserId(int userId)
        {
            var cart = await _context.Carts
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return cart == 0 ? (int?)null : cart;
        }


        public async Task<CartItemDto> AddCartItemInCart(int productId, int cartId, int quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > product.StockQuantity)
                {
                    throw new InvalidOperationException($"Requested quantity exceeds available stock. Available: {product.StockQuantity}");
                }

                existingItem.Quantity += quantity;
                existingItem.Price = product.SellingPrice;

                _context.Update(existingItem);
            }
            else
            {
                if (quantity > product.StockQuantity)
                {
                    throw new InvalidOperationException($"Requested quantity exceeds available stock. Available: {product.StockQuantity}");
                }

                var cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.SellingPrice,
                    Product = product
                };

                cart.Items.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            var updatedItem = cart.Items.First(i => i.ProductId == productId);
            return CartItemMapper.ToDto(updatedItem);
        }


        public async Task<bool> RemoveCartItemInsideOfCartByCartItemId(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                throw new InvalidOperationException("Cart item not found.");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ClearCart(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            _context.CartItems.RemoveRange(cart.Items);
            cart.Items.Clear();

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<decimal> GetCartTotal(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            decimal total = cart.Items.Sum(item => item.Price * item.Quantity);

            return total;
        }
        public async Task<decimal> GetShippingPrice(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            decimal totalPrice = cartItems.Sum(ci => ci.Price * ci.Quantity);
            if (totalPrice < 5000)
                return 100;
            else
            {
                if (totalPrice >= 5000 && totalPrice < 10000)
                    return 50;
                else
                    return 0;
            }
        }
        public async Task<bool> ValidateCart(int cartId)
        {
            bool listChanged = false;
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var itemsToRemove = new List<CartItem>();

            foreach (var item in cart.Items)
            {
                if (item.Quantity > item.Product.StockQuantity)
                {
                    if (item.Product.StockQuantity > 0)
                    {
                        item.Quantity = item.Product.StockQuantity;
                        _context.Update(item);
                        listChanged = true;
                    }
                    else
                    {
                        itemsToRemove.Add(item);
                        listChanged = true;
                    }
                }
            }

            foreach (var item in itemsToRemove)
            {
                cart.Items.Remove(item);
                _context.CartItems.Remove(item);
            }

            if (itemsToRemove.Any() || cart.Items.Any(i => i.Quantity > i.Product.StockQuantity) || listChanged == true)
            {
                await _context.SaveChangesAsync();
                return false;
            }
            if (!cart.Items.Any())
            {
                return false;
            }
            return true;

        }
        public async Task<int> GetCartItemsCountByUserId(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            return cart.Items.Sum(item => item.Quantity);
        }

    }
}
