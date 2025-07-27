namespace ShopBackEnd.Data.Dto
{
    public class CustomerPurchasePatternDto
    {
        public int UserId { get; set; }
        public int TotalOrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime LastPurchaseDate { get; set; }
    }
}
