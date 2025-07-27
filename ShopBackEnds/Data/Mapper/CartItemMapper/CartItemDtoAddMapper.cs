using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

namespace ShopBackEnd.Data.Mapper.CartItemMapper
{
    public class CartItemDtoAddMapper
    {
        public static CartItem ToEntity(CartItemDtoAdd cartItemDtoAdd)
        {
            if (cartItemDtoAdd == null) return null;

            return new CartItem
            {
                CartId = cartItemDtoAdd.CartId,
                ProductId = cartItemDtoAdd.ProductId,
                Quantity = cartItemDtoAdd.Quantity,
                Price = cartItemDtoAdd.Price
            };
        }
    }
}
