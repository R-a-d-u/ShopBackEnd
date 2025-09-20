using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ShopBackEnd.Data.Enums;
using Microsoft.Extensions.Logging;
using ShopBackEnd.Data.Mapper.OrderMapper;
using ShopBackEnd.Data.Mapper.OrderItemMapper;
using ShopBackEnd.HelperClass;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShopDbContext _context;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(
            ShopDbContext context,
            ICartRepository cartRepository,
            IOrderItemRepository orderItemRepository,
            ILogger<OrderRepository> logger)
        {
            _context = context;
            _cartRepository = cartRepository;
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }

        public async Task<OrderDtoAdd> CreateOrderFromCart(int cartId, string userAddress, PaymentMethod paymentMethod)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bool isValidCart = await _cartRepository.ValidateCart(cartId);
                if (!isValidCart)
                {
                    _logger.LogWarning($"Cart with ID {cartId} is invalid or empty after validation.");
                    throw new InvalidOperationException("Cart is invalid or empty.");
                }

                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.Id == cartId);

                if (cart == null || !cart.Items.Any())
                {
                    _logger.LogWarning($"Cart with ID {cartId} is empty or not found.");
                    throw new InvalidOperationException("Cart not found or is empty.");
                }

                var order = new Order
                {
                    Id = SequentialGuidGenerator.NewGuid(),
                    UserId = cart.UserId,
                    TotalSum = 0,
                    ShippingFee = 0,
                    Address = userAddress,
                    OrderStatus = OrderStatus.Created,
                    PaymentMethod = paymentMethod,
                    OrderCreatedDate = DateTime.Now,
                    OrderItems = new List<OrderItem>()
                };

                await _context.Orders.AddAsync(order);

                var orderItems = await _orderItemRepository.CreateOrderItems(cartId, order.Id);

                if (orderItems == null || !orderItems.Any())
                {
                    throw new InvalidOperationException("Failed to create order items.");
                }

                foreach (var orderItem in orderItems)
                {
                    var entityOrderItem = OrderItemAddMapper.ToEntity(orderItem);
                    order.OrderItems.Add(entityOrderItem);
                    await _context.OrderItems.AddAsync(entityOrderItem);
                }

                decimal totalSum = Math.Round(await _cartRepository.GetCartTotal(cartId), 2);
                decimal shippingPrice = Math.Round(await _cartRepository.GetShippingPrice(cartId), 2);

                order.TotalSum = totalSum + shippingPrice;
                order.ShippingFee = shippingPrice;

                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError($"Database update error: {dbEx.Message}");
                    throw new InvalidOperationException("Database update failed.", dbEx);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error: {ex.Message}");
                    throw new InvalidOperationException("An unexpected error occurred.", ex);
                }

                foreach (var orderItem in order.OrderItems)
                {
                    var saleRecord = new SaleRecord
                    {
                        OrderItemId = orderItem.Id,
                        OrderId = order.Id,
                        SaleDate = order.OrderCreatedDate
                    };

                    await _context.SaleRecords.AddAsync(saleRecord);
                }

                await _context.SaveChangesAsync();

                await _cartRepository.ClearCart(cartId);

                foreach (var orderItem in order.OrderItems)
                {
                    var product = await _context.Products.FindAsync(orderItem.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity -= orderItem.Quantity;

                        if (product.StockQuantity <= 0)
                        {
                            product.ProductState = ProductState.outOfStock;
                        }

                        _context.Products.Update(product);
                    }
                }

                await _context.SaveChangesAsync();

                foreach (var orderItem in order.OrderItems)
                {
                    var cartItemsToUpdate = await _context.CartItems
                        .Where(ci => ci.ProductId == orderItem.ProductId)
                        .ToListAsync();

                    foreach (var cartItem in cartItemsToUpdate)
                    {
                        if (cartItem.Quantity > cartItem.Product.StockQuantity)
                        {
                            if (cartItem.Product.StockQuantity > 0)
                            {
                                cartItem.Quantity = cartItem.Product.StockQuantity;
                            }
                            else
                            {
                                _context.CartItems.Remove(cartItem);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }

                var createdOrderItems = await _context.OrderItems
                    .Where(oi => oi.OrderId == order.Id)
                    .Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    })
                    .ToListAsync();

                var orderDto = new OrderDtoAdd
                {
                    UserId = cart.UserId,
                    TotalSum = totalSum,
                    ShippingFee = shippingPrice,
                    Address = userAddress,
                    PaymentMethod = paymentMethod,
                    OrderStatus = OrderStatus.Created,
                    OrderItems = createdOrderItems
                };

                return orderDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error occurred while creating order for cart ID {cartId}: {ex.Message}");
                throw new InvalidOperationException("An error occurred while processing your order. Please try again.");
            }
        }

        public async Task<OrderDto> GetOrderById(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product) 
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found.");
                throw new KeyNotFoundException("Order not found.");
            }

            return OrderMapper.ToDto(order);
        }

        public async Task<PagedResult<OrderDto>> GetAllOrdersByUserId(int userId, int pageNumber, int pageSize)
        {
            var query = _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderCreatedDate)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsNoTracking();

            var totalItems = await query.CountAsync();

            if (totalItems == 0)
            {
                _logger.LogWarning($"No orders found for user with ID {userId}.");
                throw new KeyNotFoundException("No orders found for this user.");
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<OrderDto>(
                items.Select(OrderMapper.ToDto),
                totalItems,
                pageNumber,
                pageSize
            );
        }
        public async Task<PagedResult<OrderDto>> GetAllOrdersByOrderStatus(OrderStatus orderStatus,int pageNumber, int pageSize)
        {
            var query = _context.Orders
                .Where(o => o.OrderStatus == orderStatus)
                .OrderBy(o => o.OrderCreatedDate)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsNoTracking();

            var totalItems = await query.CountAsync();

            if (totalItems == 0)
            {
                _logger.LogWarning($"No orders found with status {orderStatus}.");
                throw new KeyNotFoundException("No orders found with the specified status.");
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<OrderDto>(
                items.Select(OrderMapper.ToDto),
                totalItems,
                pageNumber,
                pageSize
            );
        }


        public async Task UpdateOrderStatusToProcessing(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found.");
                throw new KeyNotFoundException("Order not found.");
            }

            order.OrderStatus = OrderStatus.Processing;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrderStatusToShipping(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found.");
                throw new KeyNotFoundException("Order not found.");
            }

            order.OrderStatus = OrderStatus.Shipping;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateOrderStatusToDelivered(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found.");
                throw new KeyNotFoundException("Order not found.");
            }

            order.OrderStatus = OrderStatus.Delivered;
            order.OrderCompletedDate = DateTime.Now;   
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusToReturned(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found.");
                throw new KeyNotFoundException("Order not found.");
            }

            order.OrderStatus = OrderStatus.Returned;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusToCanceled(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found.");
                throw new KeyNotFoundException("Order not found.");
            }

            order.OrderStatus = OrderStatus.Canceled;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<int> CountCreatedAndPendingOrders()
        {
            return await _context.Orders
                .Where(o => o.OrderStatus == OrderStatus.Created ||
                            o.OrderStatus == OrderStatus.Processing)
                .CountAsync();
        }
    }
}

