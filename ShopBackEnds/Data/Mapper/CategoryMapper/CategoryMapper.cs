namespace ShopBackEnd.Data.Mapper.CategoryMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Mapper.ProductMapper;

public class CategoryMapper
{
   
    public static CategoryDto ToDto(Category category)
    {
        if (category == null) return null;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Products = category.Products?.Select(ProductMapper.ToDto).ToList() ?? new List<ProductDto>(),
            LastModifiedDate = category.LastModifiedDate
        };
    }

    
    public static Category ToEntity(CategoryDto categoryDto)
    {
        if (categoryDto == null) return null;

        return new Category
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            Products = categoryDto.Products?.Select(ProductMapper.ToEntity).ToList() ?? new List<Product>(),
            LastModifiedDate = categoryDto.LastModifiedDate
        };
    }
}
