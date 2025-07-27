namespace ShopBackEnd.Data.Dto
{
    public class ProductSalesSummaryDto
    {
        public string ProductName { get; set; } = "";
        public decimal AveragePrice { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal SellingPercentage { get; set; }
    }
}
