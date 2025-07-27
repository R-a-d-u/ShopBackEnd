namespace ShopBackEnd.Data.Dto
{
    public class ProductDtoEditStock
    {
        public int StockQuantity { get; set; }
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

    }
}
