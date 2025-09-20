using Microsoft.EntityFrameworkCore;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Repository.Context;
using ShopBackEnd.Data.Mapper.OrderItemMapper;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<OrderItemRepository> _logger;

        public OrderItemRepository(ShopDbContext context, ILogger<OrderItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<OrderItemDtoAdd>> CreateOrderItems(int cartId, Guid orderId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null || !cart.Items.Any())
            {
                _logger.LogWarning($"Cart with ID {cartId} is empty or not found.");
                return new List<OrderItemDtoAdd>();
            }

            var orderItemDtos = cart.Items.Select(cartItem => new OrderItemDtoAdd
            {
                OrderId = orderId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            }).ToList();

            _logger.LogInformation($"Successfully converted cart {cartId} to order item DTOs for order {orderId}.");

            return orderItemDtos;
        }


    }
}
