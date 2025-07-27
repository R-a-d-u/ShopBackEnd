using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class ProductDtoAdd
    {
        public ProductType ProductType { get; set; } 
        public string Name { get; set; } = "";
        public string Image { get; set; }
        public decimal AdditionalValue { get; set; } 
        public decimal GoldWeightInGrams { get; set; } 
        public decimal SellingPrice { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; } = "";
        public int StockQuantity { get; set; } 
        public ProductState ProductState { get; set; }
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    }
}
