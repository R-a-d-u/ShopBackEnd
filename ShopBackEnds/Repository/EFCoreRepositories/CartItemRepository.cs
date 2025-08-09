using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Mapper.CartItemMapper;
using ShopBackEnd.Repository.Context;
using System;
using System.Threading.Tasks;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class CartItemRepository: ICartItemRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<CartItemRepository> _logger;

        public CartItemRepository(ShopDbContext context, ILogger<CartItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartItemDto> GetCartItemById(int cartItemId)
        {
            var cartItem= await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                throw new InvalidOperationException("Cart item not found.");
            }
            return CartItemMapper.ToDto(cartItem);
        }

        public async Task<CartItemDto> UpdateCartItemQuantity(int cartItemId, int newQuantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
            {
                throw new InvalidOperationException("Cart item not found.");
            }

            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            if (newQuantity > product.StockQuantity)
            {
                throw new InvalidOperationException($"Requested quantity exceeds available stock. Available: {product.StockQuantity}");
            }

            if (newQuantity == 0)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return null; 
            }
            else
            {
                cartItem.Quantity = newQuantity;
                _context.Update(cartItem);
            }

            await _context.SaveChangesAsync();

            return CartItemMapper.ToDto(cartItem);
        }

    }
}
