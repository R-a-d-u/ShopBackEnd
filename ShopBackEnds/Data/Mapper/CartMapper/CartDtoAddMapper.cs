using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

namespace ShopBackEnd.Data.Mapper.CartMapper
{
    public class CartDtoAddMapper
    {
        public static Cart ToEntity(CartDtoAdd cartDtoAdd)
        {
            if (cartDtoAdd == null) return null;

            return new Cart
            {
                UserId = cartDtoAdd.UserId,
                Items = cartDtoAdd.Items?.Select(CartItemMapper.CartItemMapper.ToEntity).ToList() ?? new List<CartItem>()
            };
        }
    }
}
