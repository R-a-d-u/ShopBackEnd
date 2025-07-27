using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class OrderDtoQuery
    {
        public int? UserId { get; set; } 
        public DateTime? FromDate { get; set; } 
        public DateTime? ToDate { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public PaymentMethod? PaymentMethod { get; set; } 
        public decimal? MinTotalSum { get; set; } 
        public decimal? MaxTotalSum { get; set; } 
    }
}
