using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Services;
using ShopBackEnd.Validation.ProductValidation;
using FluentValidation;
using ShopBackEnd.HelperClass;
using ShopBackEnd.Data.Entity;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductIdValidation _productIdValidator;
    private readonly ProductDtoAddValidation _addProductValidator;
    private readonly ProductEditInformationValidation _editProductInformationValidator;
    private readonly ProductEditSellingPriceValidation _editProductSellingPriceValidator;
    private readonly ProductEditStockValidation _editProductStockValidator;
    private readonly ProductEditStateValidation _editProductStateValidator;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(
        IProductRepository productRepository,
        ProductIdValidation productIdValidator,
        ProductDtoAddValidation addProductValidator,
        ProductEditInformationValidation editProductInformationValidator,
        ProductEditSellingPriceValidation editProductSellingPriceValidator,
        ProductEditStockValidation editProductStockValidator,
        ProductEditStateValidation editProductStateValidator,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _productIdValidator = productIdValidator;
        _addProductValidator = addProductValidator;
        _editProductInformationValidator = editProductInformationValidator;
        _editProductSellingPriceValidator = editProductSellingPriceValidator;
        _editProductStockValidator = editProductStockValidator;
        _editProductStateValidator = editProductStateValidator;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto?> GetProductById(int id)
    {
        var validationResult = _productIdValidator.Validate(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await _productRepository.GetProductById(id);
    }

    public async Task<PagedResult<ProductDto>> GetAllDiscontinuedProducts(int pageNumber, int pageSize)
    {
        return await _productRepository.GetAllDiscontinuedProducts(pageNumber, pageSize);
    }

    public async Task<PagedResult<ProductDto>> GetAllProductsByCategoryId(int categoryId, int pageNumber, int pageSize)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
        }

        return await _productRepository.GetAllProductsByCategoryId(categoryId, pageNumber, pageSize);
       
    }

    public async Task<PagedResult<ProductDto>> GetAllProductsByName(string name, int pageNumber, int pageSize)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Search name cannot be empty.");
        }

        return await _productRepository.GetAllProductsByName(name, pageNumber, pageSize);
        
    }

    public async Task<ProductDtoEditInformation?> GetProductInformationById(int id)
    {
        var validationResult = _productIdValidator.Validate(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await _productRepository.GetProductInformationById(id);
    }

    public async Task<bool> SetStateToOutOfStock(int id, ProductDtoEditState dto)
    {
        var validationResult = _productIdValidator.Validate(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var stateValidationResult = _editProductStateValidator.Validate(dto);
        if (!stateValidationResult.IsValid)
        {
            throw new ValidationException(stateValidationResult.Errors);
        }

        return await _productRepository.SetStateToOutOfStock(id, dto);
    }

    public async Task<bool> SetStateToDiscontinued(int id, ProductDtoEditState dto)
    {
        var validationResult = _productIdValidator.Validate(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var stateValidationResult = _editProductStateValidator.Validate(dto);
        if (!stateValidationResult.IsValid)
        {
            throw new ValidationException(stateValidationResult.Errors);
        }

        return await _productRepository.SetStateToDiscontinued(id, dto);
    }

    public async Task<bool> SetStateToInStock(int id, ProductDtoEditState dto)
    {
        var validationResult = _productIdValidator.Validate(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var stateValidationResult = _editProductStateValidator.Validate(dto);
        if (!stateValidationResult.IsValid)
        {
            throw new ValidationException(stateValidationResult.Errors);
        }

        return await _productRepository.SetStateToInStock(id, dto);
    }

    public async Task<bool> EditProductStockQuantity(int id, ProductDtoEditStock dto)
    {
        var validationResult = _productIdValidator.Validate(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var stockValidationResult = _editProductStockValidator.Validate(dto);
        if (!stockValidationResult.IsValid)
        {
            throw new ValidationException(stockValidationResult.Errors);
        }

        return await _productRepository.EditProductStockQuantity(id, dto);
    }

    public async Task<bool> EditProductAdditionalPrice(int id, ProductDtoEditSellingPrice dto)
    {
        var validationResult = _productIdValidator.Validate(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var priceValidationResult = _editProductSellingPriceValidator.Validate(dto);
        if (!priceValidationResult.IsValid)
        {
            throw new ValidationException(priceValidationResult.Errors);
        }

        return await _productRepository.EditProductAdditionalPrice(id, dto);
    }

    public async Task<bool> EditProductInformation(int id, ProductDtoEditInformation dto)
    {
        var validationResult = _productIdValidator.Validate(id);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var infoValidationResult = _editProductInformationValidator.Validate(dto);
        if (!infoValidationResult.IsValid)
        {
            throw new ValidationException(infoValidationResult.Errors);
        }

        var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {dto.CategoryId} not found.");
        }

        return await _productRepository.EditProductInformation(id, dto);
    }

    public async Task<int> AddProduct(ProductDtoAdd dto)
    {
        var validationResult = _addProductValidator.Validate(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {dto.CategoryId} not found.");
        }

        return await _productRepository.AddProduct(dto);
    }
    public async Task<bool> UpdateAllGoldProductPrices()
    {

        return await _productRepository.UpdateAllGoldProductPrices();
    }

    public async Task<List<ProductDto>> FilterAllProductsByCategory(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
        }

        return await _productRepository.FilterAllProductsByCategory(categoryId);
    }

    public async Task<List<ProductDto>> FilterAllProducts(ProductDtoFilter filter)
    {
        if (filter.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(filter.CategoryId.Value);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {filter.CategoryId} not found.");
            }
        }

        return await _productRepository.FilterAllProducts(filter);
    }
    public async Task<int> GetTotalNonDiscontinuedProductsCount()
    {
        return await _productRepository.GetTotalNonDiscontinuedProductsCount();
    }
    public async Task<int> GetTotalLowStockProductsCount()
    {
        return await _productRepository.GetTotalLowStockProductsCount();
    }
}