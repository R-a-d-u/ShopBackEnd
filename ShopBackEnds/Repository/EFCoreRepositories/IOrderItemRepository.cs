using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItemDtoAdd>> CreateOrderItems(int cartId, Guid orderId);
    }
}
