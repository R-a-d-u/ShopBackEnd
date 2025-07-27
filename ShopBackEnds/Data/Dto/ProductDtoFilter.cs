using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class ProductDtoFilter
    {
        public string? Name { get; set; } 
        public int? CategoryId { get; set; } 
        public decimal? MinPrice { get; set; } 
        public decimal? MaxPrice { get; set; } 
        public ProductState? ProductState { get; set; } 
    }
}
