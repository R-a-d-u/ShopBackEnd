using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Mapper.ProductMapper;

namespace ShopBackEnd.Data.Mapper.CartItemMapper
{
    public class CartItemMapper
    {
        public static CartItemDto ToDto(CartItem cartItem)
        {
            if (cartItem == null) return null;

            return new CartItemDto
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Product = ProductMapper.ProductMapper.ToDto(cartItem.Product),
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };
        }

        // Maps CartItemDto to CartItem entity
        public static CartItem ToEntity(CartItemDto cartItemDto)
        {
            if (cartItemDto == null) return null;

            return new CartItem
            {
                Id = cartItemDto.Id,
                CartId = cartItemDto.CartId,
                ProductId = cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity,
                Price = cartItemDto.Price
            };
        }
    }
}
