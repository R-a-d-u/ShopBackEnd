using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class OrderDtoAdd
    {
        public int UserId { get; set; }
        public DateTime OrderCreatedDate { get; set; }= DateTime.Now;
        public decimal TotalSum { get; set; }
        public decimal ShippingFee { get; set; }
        public string Address { get; set; } = "";
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }= OrderStatus.Created;
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
