using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

namespace ShopBackEnd.Data.Mapper.CartItemMapper
{
    public class CartItemDtoEditQuantityMapper
    {
        public static CartItem ToEntity(CartItemDtoEditQuantity cartItemDtoEditQuantity, int cartItemId)
        {
            if (cartItemDtoEditQuantity == null) return null;

            return new CartItem
            {
                Id = cartItemId,
                Quantity = cartItemDtoEditQuantity.Quantity
            };
        }
    }
}
