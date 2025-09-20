using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Validators.ResponseValidator;
using ShopBackEnd.Service;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("GetAllNames", Name = "GetAllCategoryNames")]
    public async Task<ActionResult<ResponseValidator<IEnumerable<CategoryNameDto>>>> GetAllCategoryNames()
    {
        try
        {
            var categoryNames = await _categoryService.GetAllCategoryNamesAsync();
            if (categoryNames == null || !categoryNames.Any())
            {
                return NotFound(ResponseValidator<IEnumerable<CategoryNameDto>>.Failure("No categories found."));
            }
            return Ok(ResponseValidator<IEnumerable<CategoryNameDto>>.Success(categoryNames));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<IEnumerable<CategoryNameDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetAll", Name = "GetAllCategories")]
    public async Task<ActionResult<ResponseValidator<IEnumerable<CategoryDto>>>> GetAllCategories()
    {
        try
        {
            var categoryList = await _categoryService.GetAllCategoriesAsync();
            if (categoryList == null || !categoryList.Any())
            {
                return NotFound(ResponseValidator<IEnumerable<CategoryDto>>.Failure("The category list is empty."));
            }
            return Ok(ResponseValidator<IEnumerable<CategoryDto>>.Success(categoryList));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<IEnumerable<CategoryDto>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<IEnumerable<CategoryDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("Get/{id}", Name = "GetCategoryById")]
    public async Task<ActionResult<ResponseValidator<CategoryDto>>> GetCategoryById(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(ResponseValidator<CategoryDto>.Success(category));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<CategoryDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<CategoryDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<CategoryDto>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("GetName/{id}", Name = "GetCategoryNameById")]
    public async Task<ActionResult<ResponseValidator<string>>> GetCategoryNameById(int id)
    {
        try
        {
            var name = await _categoryService.GetCategoryNameByIdAsync(id);
            return Ok(ResponseValidator<string>.Success(name));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<string>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<string>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<string>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPost("Add", Name = "AddCategory")]
    public async Task<ActionResult<ResponseValidator<CategoryDto>>> AddCategory([FromBody] CategoryDtoAdd category)
    {
        try
        {
            var addedCategory = await _categoryService.AddCategoryAsync(category);
            return Ok(ResponseValidator<CategoryDto>.Success(addedCategory));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<CategoryDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<CategoryDto>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("EditName/{categoryId}", Name = "EditCategoryName")]
    public async Task<ActionResult<ResponseValidator<CategoryDto>>> EditCategoryName(int categoryId, [FromBody] CategoryDtoEdit categoryEdit)
    {
        try
        {
            var updatedCategory = await _categoryService.EditCategoryNameAsync(categoryId, categoryEdit);
            return Ok(ResponseValidator<CategoryDto>.Success(updatedCategory));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<CategoryDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<CategoryDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<CategoryDto>.Failure($"An error occurred: {e.Message}"));
        }
    }


    [HttpDelete("Delete/{categoryId}", Name = "DeleteCategory")]
    public async Task<ActionResult<ResponseValidator<bool>>> DeleteCategory(int categoryId)
    {
        try
        {
            var result = await _categoryService.DeleteCategoryAsync(categoryId);
            if (!result)
            {
                return BadRequest(ResponseValidator<bool>.Failure($"The category with ID {categoryId} could not be deleted."));
            }
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
}
