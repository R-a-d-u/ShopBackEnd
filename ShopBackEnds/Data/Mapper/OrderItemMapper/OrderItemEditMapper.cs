namespace ShopBackEnd.Data.Mapper.OrderItemMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class OrderItemEditMapper
{
    
    public static void UpdateEntity(OrderItem orderItem, OrderItemEditDto orderItemEditDto)
    {
        if (orderItem == null || orderItemEditDto == null) return;

        orderItem.Quantity = orderItemEditDto.Quantity;
        orderItem.Price = orderItemEditDto.Price;
    }
}