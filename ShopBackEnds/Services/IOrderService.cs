using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.HelperClass;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBackEnd.Services
{
    public interface IOrderService
    {
        Task<OrderDtoAdd> CreateOrderFromCart(int cartId, string userAddress, PaymentMethod paymentMethod);
        Task<OrderDto> GetOrderById(Guid orderId);
        Task<PagedResult<OrderDto>> GetAllOrdersByUserId(int userId, int pageNumber, int pageSize);
        Task<PagedResult<OrderDto>> GetAllOrdersByOrderStatus(OrderStatus orderStatus,int pageNumber, int pageSize);
        Task UpdateOrderStatusToProcessing(Guid orderId);
        Task UpdateOrderStatusToDelivered(Guid orderId);
        Task UpdateOrderStatusToShipping(Guid orderId);
        Task UpdateOrderStatusToReturned(Guid orderId);
        Task UpdateOrderStatusToCanceled(Guid orderId);
        Task<int> CountCreatedAndPendingOrders();
    }
}
