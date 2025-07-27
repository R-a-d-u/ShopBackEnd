using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Mapper.OrderItemMapper;

public class OrderAddMapper
{
    
    public static Order ToEntity(OrderDtoAdd orderDtoAdd)
    {
        if (orderDtoAdd == null) return null;

        return new Order
        {
            UserId = orderDtoAdd.UserId,
            OrderCreatedDate = orderDtoAdd.OrderCreatedDate,
            TotalSum = orderDtoAdd.TotalSum,
            ShippingFee = orderDtoAdd.ShippingFee,
            Address = orderDtoAdd.Address,
            PaymentMethod = orderDtoAdd.PaymentMethod,
            OrderStatus = orderDtoAdd.OrderStatus,
            OrderItems = orderDtoAdd.OrderItems.Select(OrderItemMapper.ToEntity).ToList()
        };
    }
}
