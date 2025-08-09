using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

namespace ShopBackEnd.Services
{
    public interface ICartItemService
    {
        Task<CartItemDto> GetCartItemById(int cartItemId);
        Task<CartItemDto> UpdateCartItemQuantity(int cartItemId, int newQuantity);
    }
}
