namespace ShopBackEnd.Data.Mapper.OrderItemMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Mapper.ProductMapper;

public class OrderItemMapper
{
    
    public static OrderItemDto ToDto(OrderItem orderItem)
    {
        if (orderItem == null) return null;
        return new OrderItemDto
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            Price = orderItem.Price,
            Product = orderItem.Product != null ? ProductMapper.ToDto(orderItem.Product) : null
        };
    }

   
    public static OrderItem ToEntity(OrderItemDto orderItemDto)
    {
        if (orderItemDto == null) return null;
        return new OrderItem
        {
            Id = orderItemDto.Id,
            OrderId = orderItemDto.OrderId,
            ProductId = orderItemDto.ProductId,
            Quantity = orderItemDto.Quantity,
            Price = orderItemDto.Price,
            Product = orderItemDto.Product != null ? ProductMapper.ToEntity(orderItemDto.Product) : null
        };
    }
}