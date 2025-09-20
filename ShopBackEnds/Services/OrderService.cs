using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.HelperClass;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Validation.OrderValidation; // Assuming you have similar validations for Orders
using ShopBackEnd.Validations;

namespace ShopBackEnd.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly OrderDtoAddValidation _orderDtoAddValidator;
        private readonly OrderIdValidation _orderIdValidator;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IOrderItemRepository orderItemRepository,
            OrderDtoAddValidation orderDtoAddValidator,
            OrderIdValidation orderIdValidator,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _orderItemRepository = orderItemRepository;
            _orderDtoAddValidator = orderDtoAddValidator;
            _orderIdValidator = orderIdValidator;
            _logger = logger;
        }

        public async Task<OrderDtoAdd> CreateOrderFromCart(int cartId, string userAddress, PaymentMethod paymentMethod)
        {

            var orderDto = await _orderRepository.CreateOrderFromCart(cartId, userAddress, paymentMethod);

            var validationResult = _orderDtoAddValidator.Validate(orderDto); 
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors); 
            }

            return orderDto; 
        }

        public async Task<OrderDto> GetOrderById(Guid orderId)
        {
            var validationResult = _orderIdValidator.Validate(orderId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _orderRepository.GetOrderById(orderId);
        }

        public async Task<PagedResult<OrderDto>> GetAllOrdersByUserId(int userId, int pageNumber, int pageSize)
        {
            return await _orderRepository.GetAllOrdersByUserId(userId, pageNumber, pageSize);
        }
        public async Task<PagedResult<OrderDto>> GetAllOrdersByOrderStatus(OrderStatus orderStatus,int pageNumber, int pageSize)
        {
            return await _orderRepository.GetAllOrdersByOrderStatus(orderStatus,pageNumber, pageSize);
        }

        public async Task UpdateOrderStatusToProcessing(Guid orderId)
        {
            var validationResult = _orderIdValidator.Validate(orderId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _orderRepository.UpdateOrderStatusToProcessing(orderId);
        }

        public async Task UpdateOrderStatusToDelivered(Guid orderId)
        {
            var validationResult = _orderIdValidator.Validate(orderId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _orderRepository.UpdateOrderStatusToDelivered(orderId);
        }
        public async Task UpdateOrderStatusToShipping(Guid orderId)
        {
            var validationResult = _orderIdValidator.Validate(orderId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _orderRepository.UpdateOrderStatusToShipping(orderId);
        }

      
        public async Task UpdateOrderStatusToReturned(Guid orderId)
        {
            var validationResult = _orderIdValidator.Validate(orderId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _orderRepository.UpdateOrderStatusToReturned(orderId);
        }

       
        public async Task UpdateOrderStatusToCanceled(Guid orderId)
        {
            var validationResult = _orderIdValidator.Validate(orderId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _orderRepository.UpdateOrderStatusToCanceled(orderId);
        }
        public async Task<int> CountCreatedAndPendingOrders()
        {
            return await _orderRepository.CountCreatedAndPendingOrders();
        }

        
    }

}
