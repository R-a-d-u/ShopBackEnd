namespace ShopBackEnd.Data.Dto
{
    public class GoldHistoryDto
    {
        public int Id { get; set; }
        public string Metal { get; set; } = "";
        public decimal PriceOunce { get; set; }
        public decimal PriceGram { get; set; }
        public double PercentageChange { get; set; }
        public string Exchange { get; set; } = "";
        public string Timestamp { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
