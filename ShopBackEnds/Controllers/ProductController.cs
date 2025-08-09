using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Validators.ResponseValidator;
using ShopBackEnd.Services;
using ShopBackEnd.HelperClass;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopBackEnd.Data.Entity;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("Get/{id}", Name = "GetProductById")]
    public async Task<ActionResult<ResponseValidator<ProductDto>>> GetProductById(int id)
    {
        try
        {
            var product = await _productService.GetProductById(id);
            return Ok(ResponseValidator<ProductDto>.Success(product));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<ProductDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<ProductDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<ProductDto>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetAllDiscontinued", Name = "GetAllDiscontinuedProducts")]
    public async Task<ActionResult<ResponseValidator<PagedResult<ProductDto>>>> GetAllDiscontinuedProducts([FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedResult = await _productService.GetAllDiscontinuedProducts(page, 10);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<ProductDto>>.Failure("The product list is empty."));
            }
            return Ok(ResponseValidator<PagedResult<ProductDto>>.Success(pagedResult));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<(List<ProductDto> Products, int TotalCount)>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetByCategory/{categoryId}", Name = "GetProductsByCategoryId")]
    public async Task<ActionResult<ResponseValidator<PagedResult<ProductDto>>>> GetProductsByCategoryId(
     int categoryId,
     [FromQuery(Name = "page")] int page = 1,
     [FromQuery(Name = "pageSize")] int pageSize = 4) // Allow page size as a parameter
    {
        try
        {
            var pagedResult = await _productService.GetAllProductsByCategoryId(categoryId, page, pageSize);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<ProductDto>>.Failure("The product list is empty."));
            }
            return Ok(ResponseValidator<PagedResult<ProductDto>>.Success(pagedResult));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<PagedResult<ProductDto>>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<ProductDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetByName", Name = "GetProductsByName")]
    public async Task<ActionResult<ResponseValidator<PagedResult<ProductDto>>>> GetProductsByName(string name, [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedResult = await _productService.GetAllProductsByName(name, page, 5);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<ProductDto>>.Failure("The product list is empty."));
            }
            return Ok(ResponseValidator<PagedResult<ProductDto>>.Success(pagedResult));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<(List<ProductDto> Products, int TotalCount)>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<(List<ProductDto> Products, int TotalCount)>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPost("Add", Name = "AddProduct")]
    public async Task<ActionResult<ResponseValidator<int>>> AddProduct([FromBody] ProductDtoAdd product)
    {
        try
        {
            var addedProductId = await _productService.AddProduct(product);
            return Ok(ResponseValidator<int>.Success(addedProductId));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<int>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<int>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<int>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("EditInformation/{id}", Name = "EditProductInformation")]
    public async Task<ActionResult<ResponseValidator<bool>>> EditProductInformation(int id, [FromBody] ProductDtoEditInformation dto)
    {
        try
        {
            var result = await _productService.EditProductInformation(id, dto);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("EditStock/{id}", Name = "EditProductStock")]
    public async Task<ActionResult<ResponseValidator<bool>>> EditProductStock(int id, [FromBody] ProductDtoEditStock dto)
    {
        try
        {
            var result = await _productService.EditProductStockQuantity(id, dto);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("EditPrice/{id}", Name = "EditProductPrice")]
    public async Task<ActionResult<ResponseValidator<bool>>> EditProductPrice(int id, [FromBody] ProductDtoEditSellingPrice dto)
    {
        try
        {
            var result = await _productService.EditProductAdditionalPrice(id, dto);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("UpdateGoldProductPrices", Name = "UpdateGoldProductPrices")]
    public async Task<ActionResult<ResponseValidator<bool>>> UpdateGoldProductPrices()
    {
        try
        {
            var result = await _productService.UpdateAllGoldProductPrices();
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpPut("SetInStock/{id}", Name = "SetProductInStock")]
    public async Task<ActionResult<ResponseValidator<bool>>> SetInStock(int id, [FromBody] ProductDtoEditState dto)
    {
        try
        {
            var result = await _productService.SetStateToInStock(id, dto);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("SetOutOfStock/{id}", Name = "SetProductOutOfStock")]
    public async Task<ActionResult<ResponseValidator<bool>>> SetOutOfStock(int id, [FromBody] ProductDtoEditState dto)
    {
        try
        {
            var result = await _productService.SetStateToOutOfStock(id, dto);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("SetDiscontinued/{id}", Name = "SetProductDiscontinued")]
    public async Task<ActionResult<ResponseValidator<bool>>> SetDiscontinued(int id, [FromBody] ProductDtoEditState dto)
    {
        try
        {
            var result = await _productService.SetStateToDiscontinued(id, dto);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("TotalNonDiscontinuedProducts", Name = "GetTotalNonDiscontinuedProducts")]
    public async Task<ActionResult<ResponseValidator<int>>> GetTotalNonDiscontinuedProducts()
    {
        try
        {
            var totalCount = await _productService.GetTotalNonDiscontinuedProductsCount();
            return Ok(ResponseValidator<int>.Success(totalCount));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<int>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("LowStockProductsCount", Name = "GetLowStockProductsCount")]
    public async Task<ActionResult<ResponseValidator<int>>> GetLowStockProductsCount()
    {
        try
        {
            var lowStockCount = await _productService.GetTotalLowStockProductsCount();
            return Ok(ResponseValidator<int>.Success(lowStockCount));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<int>.Failure($"An error occurred: {e.Message}"));
        }
    }


}
