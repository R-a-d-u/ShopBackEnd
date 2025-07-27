namespace ShopBackEnd.Data.Dto
{
    public class OrderItemDtoAdd
    {
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
