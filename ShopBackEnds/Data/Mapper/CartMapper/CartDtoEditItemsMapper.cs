using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

namespace ShopBackEnd.Data.Mapper.CartMapper
{
    public class CartDtoEditItemsMapper
    {
        public static Cart ToEntity(CartDtoEditItems cartDtoEditItems)
        {
            if (cartDtoEditItems == null) return null;

            return new Cart
            {
                Items = cartDtoEditItems.Items?.Select(CartItemMapper.CartItemMapper.ToEntity).ToList() ?? new List<CartItem>()
            };
        }
    }
}
