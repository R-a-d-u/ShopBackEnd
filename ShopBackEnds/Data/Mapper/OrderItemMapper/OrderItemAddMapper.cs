using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class OrderItemAddMapper
{
    
    public static OrderItem ToEntity(OrderItemDtoAdd orderItemDtoAdd)
    {
        if (orderItemDtoAdd == null) return null;

        return new OrderItem
        {
            OrderId = orderItemDtoAdd.OrderId,
            ProductId = orderItemDtoAdd.ProductId,
            Quantity = orderItemDtoAdd.Quantity,
            Price = orderItemDtoAdd.Price
        };
    }
}
