using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public interface ICategoryRepository
    {
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<List<CategoryNameDto>> GetAllCategoryNamesAsync();
        Task<string> GetCategoryNameByIdAsync(int id);
        Task<CategoryDto> AddCategoryAsync(CategoryDtoAdd categoryDto);
        Task<CategoryDto> EditCategoryNameAsync(int categoryId, CategoryDtoEdit categoryDtoEdit);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}