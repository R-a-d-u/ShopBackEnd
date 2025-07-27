using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int UserId { get; set; }
        public DateTime OrderCreatedDate { get; set; }
        public DateTime? OrderCompletedDate { get; set; }
        public decimal TotalSum { get; set; }
        public decimal ShippingFee { get; set; }
        public string Address { get; set; } = "";
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
