namespace ShopBackEnd.Data.Mapper.CategoryMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class CategoryEditMapper
{
    
    public static void UpdateEntity(Category category, CategoryDtoEdit categoryDtoEdit)
    {
        if (category == null || categoryDtoEdit == null) return;

        category.Name = categoryDtoEdit.Name;
        category.LastModifiedDate = categoryDtoEdit.LastModifiedDate;
    }
}
