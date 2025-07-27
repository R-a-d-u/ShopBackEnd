namespace ShopBackEnd.Data.Mapper.OrderMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Mapper.OrderItemMapper;

public class OrderMapper
{
     
    public static OrderDto ToDto(Order order)
    {
        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderCreatedDate = order.OrderCreatedDate,
            OrderCompletedDate = order.OrderCompletedDate,
            TotalSum = order.TotalSum,
            ShippingFee = order.ShippingFee,
            Address = order.Address,
            PaymentMethod = order.PaymentMethod,
            OrderStatus = order.OrderStatus,
            OrderItems = order.OrderItems.Select(OrderItemMapper.ToDto).ToList()
        };
    }

   
    public static Order ToEntity(OrderDto orderDto)
    {
        if (orderDto == null) return null;

        return new Order
        {
            Id = orderDto.Id,
            UserId = orderDto.UserId,
            OrderCreatedDate = orderDto.OrderCreatedDate,
            OrderCompletedDate = orderDto.OrderCompletedDate,
            TotalSum = orderDto.TotalSum,
            ShippingFee = orderDto.ShippingFee,
            Address = orderDto.Address,
            PaymentMethod = orderDto.PaymentMethod,
            OrderStatus = orderDto.OrderStatus,
            OrderItems = orderDto.OrderItems.Select(OrderItemMapper.ToEntity).ToList()
        };
    }
}
