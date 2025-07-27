namespace ShopBackEnd.Data.Dto
{
    public class SaleRecordDto
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public int OrderItemId { get; set; }
        public OrderDto Order { get; set; }
        public OrderItemDto OrderItem { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
