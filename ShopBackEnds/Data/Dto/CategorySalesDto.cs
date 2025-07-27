namespace ShopBackEnd.Data.Dto
{
    public class CategorySalesDto
    {
        public string CategoryName { get; set; } = "";
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal SalesPercentage { get; set; } = 0;
    }
}
