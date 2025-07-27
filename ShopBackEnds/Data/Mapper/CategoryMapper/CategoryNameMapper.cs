using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

namespace ShopBackEnd.Data.Mapper.CategoryMapper
{
    public class CategoryNameMapper
    {
        public static CategoryNameDto ToCategoryNameDto(Category category)
        {
            if (category == null) return null;

            return new CategoryNameDto
            {
                Id = category.Id,
                Name = category.Name,
                LastModifiedDate = category.LastModifiedDate
        };
        }
    }
}
