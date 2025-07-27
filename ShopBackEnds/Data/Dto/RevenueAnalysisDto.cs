namespace ShopBackEnd.Data.Dto
{
    public class RevenueAnalysisDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalOrderCount { get; set; }
        public decimal DailyAverageRevenue { get; set; }
        public int TotalConfirmedClients { get; set; }
        public int NewClientsInTimeframe { get; set; }
    }
}
