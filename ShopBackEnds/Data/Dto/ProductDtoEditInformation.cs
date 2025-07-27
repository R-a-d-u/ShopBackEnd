using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class ProductDtoEditInformation
    {
        public string Name { get; set; } = "";
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; } = "";
        public ProductType ProductType { get; set; } 
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    }
}
