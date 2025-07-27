namespace ShopBackEnd.Data.Mapper.CategoryMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class CategoryAddMapper
{
    public static Category ToEntity(CategoryDtoAdd categoryDtoAdd)
    {
        if (categoryDtoAdd == null) return null;

        return new Category
        {
            Name = categoryDtoAdd.Name,
            LastModifiedDate = categoryDtoAdd.LastModifiedDate
        };
    }
}
