namespace ShopBackEnd.Data.Dto
{
    public class ProductDtoEditSellingPrice
    {
        public decimal AdditionalValue { get; set; }
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    }
}
