using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

namespace ShopBackEnd.Data.Mapper.CartMapper
{
    public class CartMapper
    {
        public static CartDto ToDto(Cart cart)
        {
            if (cart == null) return null;

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items?.Select(CartItemMapper.CartItemMapper.ToDto).ToList() ?? new List<CartItemDto>()
            };
        }

        public static Cart ToEntity(CartDto cartDto)
        {
            if (cartDto == null) return null;

            return new Cart
            {
                Id = cartDto.Id,
                UserId = cartDto.UserId,
                Items = cartDto.Items?.Select(CartItemMapper.CartItemMapper.ToEntity).ToList() ?? new List<CartItem>()
            };
        }
    }
}
