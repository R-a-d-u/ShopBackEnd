// ICartRepository interface
using ShopBackEnd.Data.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public interface ICartRepository
    {
        Task<List<CartItemDto>> GetAllCartItems(int cartId);
        Task<int?> GetCartIdByUserId(int userId);
        Task<CartItemDto> AddCartItemInCart(int productId, int cartId, int quantity);
        Task<bool> RemoveCartItemInsideOfCartByCartItemId(int cartItemId);
        Task<bool> ClearCart(int cartId);
        Task<decimal> GetCartTotal(int cartId);
        Task<decimal> GetShippingPrice(int cartId);
        Task<bool> ValidateCart(int cartId);
        Task<int> GetCartItemsCountByUserId(int userId);
    }
}