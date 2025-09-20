using ShopBackEnd.Data.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBackEnd.Service
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<List<CategoryNameDto>> GetAllCategoryNamesAsync();
        Task<string> GetCategoryNameByIdAsync(int id);
        Task<CategoryDto> AddCategoryAsync(CategoryDtoAdd categoryDtoAdd);
        Task<CategoryDto> EditCategoryNameAsync(int categoryId, CategoryDtoEdit categoryDtoEdit);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}