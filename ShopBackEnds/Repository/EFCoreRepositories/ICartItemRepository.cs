using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public interface ICartItemRepository
    {
        Task<CartItemDto> GetCartItemById(int cartItemId);
        Task<CartItemDto> UpdateCartItemQuantity(int cartItemId, int newQuantity);
    }
}
