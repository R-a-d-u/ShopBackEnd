namespace ShopBackEnd.Data.Dto
{
    public class SaleRecordDtoAdd
    {
        public Guid OrderId { get; set; }
        public int OrderItemId { get; set; }
        public OrderDto Order { get; set; }
        public OrderItemDto OrderItem { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
