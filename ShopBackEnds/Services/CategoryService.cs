using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Validation.CategoryValidation;
using ShopBackEnd.Validations;

namespace ShopBackEnd.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryIdValidation _categoryIdValidator;
        private readonly CategoryAddValidation _addCategoryValidator;
        private readonly CategoryEditValidation _editCategoryNameValidator;
        private readonly IUserRepository _userRepository;

        public CategoryService(
            ICategoryRepository categoryRepository,
            CategoryIdValidation categoryIdValidator,
            CategoryAddValidation addCategoryValidator,
            CategoryEditValidation editCategoryNameValidator,
            IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryIdValidator = categoryIdValidator;
            _addCategoryValidator = addCategoryValidator;
            _editCategoryNameValidator = editCategoryNameValidator;
            _userRepository = userRepository;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var validationResult = _categoryIdValidator.Validate(id);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            return category;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
        public async Task<List<CategoryNameDto>> GetAllCategoryNamesAsync()
        {
            return await _categoryRepository.GetAllCategoryNamesAsync();
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDtoAdd categoryDtoAdd)
        {
            var validationResult = _addCategoryValidator.Validate(categoryDtoAdd);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await _categoryRepository.AddCategoryAsync(categoryDtoAdd);
        }

        public async Task<CategoryDto> EditCategoryNameAsync(int categoryId, CategoryDtoEdit categoryDtoEdit)
        {
            var validationResult = _categoryIdValidator.Validate(categoryId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var categoryEditValidationResult = _editCategoryNameValidator.Validate(categoryDtoEdit);
            if (!categoryEditValidationResult.IsValid)
            {
                throw new ValidationException(categoryEditValidationResult.Errors);
            }

            var category = await _categoryRepository.EditCategoryNameAsync(categoryId, categoryDtoEdit);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
            }

            return category;
        }
        public async Task<string> GetCategoryNameByIdAsync(int id)
        {
            var validationResult = _categoryIdValidator.Validate(id);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var name = await _categoryRepository.GetCategoryNameByIdAsync(id);
            if (name == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            return name;
        }


        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var validationResult = _categoryIdValidator.Validate(categoryId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _categoryRepository.DeleteCategoryAsync(categoryId);
        }
    }
}