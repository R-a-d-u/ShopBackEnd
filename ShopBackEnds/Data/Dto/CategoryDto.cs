namespace ShopBackEnd.Data.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public DateTime LastModifiedDate { get; set; }
    }
}
