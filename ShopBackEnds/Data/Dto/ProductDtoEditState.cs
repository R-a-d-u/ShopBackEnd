using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class ProductDtoEditState
    {
        public ProductState ProductState { get; set; }
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    }
}
