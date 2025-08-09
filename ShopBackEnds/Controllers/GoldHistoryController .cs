using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Validators.ResponseValidator;
using ShopBackEnd.Service;

[ApiController]
[Route("[controller]")]
public class GoldHistoryController : ControllerBase
{
    private readonly IGoldHistoryService _goldHistoryService;

    public GoldHistoryController(IGoldHistoryService goldHistoryService)
    {
        _goldHistoryService = goldHistoryService;
    }

    [HttpGet("Get/{id}", Name = "GetGoldHistoryById")]
    public async Task<ActionResult<ResponseValidator<GoldHistoryDto>>> GetGoldHistoryById(int id)
    {
        try
        {
            var goldHistory = await _goldHistoryService.GetGoldHistoryByIdAsync(id);
            return Ok(ResponseValidator<GoldHistoryDto>.Success(goldHistory));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<GoldHistoryDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<GoldHistoryDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<GoldHistoryDto>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("GetBetweenDates", Name = "GetGoldPricesBetweenDates")]
    public async Task<ActionResult<ResponseValidator<IEnumerable<GoldHistoryDto>>>> GetGoldPricesBetweenDates(
    [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var goldHistoryList = await _goldHistoryService.GetGoldPricesBetweenDatesAsync(startDate, endDate);
            if (goldHistoryList == null || !goldHistoryList.Any())
            {
                return NotFound(ResponseValidator<IEnumerable<GoldHistoryDto>>.Failure("No gold history records found in the specified date range."));
            }

            return Ok(ResponseValidator<IEnumerable<GoldHistoryDto>>.Success(goldHistoryList));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<IEnumerable<GoldHistoryDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("GetLastGoldPriceHistory", Name = "GetLastGoldPriceHistory")]
    public async Task<ActionResult<ResponseValidator<GoldHistoryDto>>> GetLastGoldPriceHistory()
    {
        try
        {
            var goldHistory = await _goldHistoryService.GetLastGoldPriceHistoryAsync();
            return Ok(ResponseValidator<GoldHistoryDto>.Success(goldHistory));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<GoldHistoryDto>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetLastPriceInGrams", Name = "GetLastPriceInGrams")]
    public async Task<ActionResult<ResponseValidator<decimal>>> GetLastPriceInGrams()
    {
        try
        {
            var priceInGrams = await _goldHistoryService.GetLastPriceInGramsAsync();
            return Ok(ResponseValidator<decimal>.Success(priceInGrams));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<decimal>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetLast7", Name = "GetLast7GoldHistory")]
    public async Task<ActionResult<ResponseValidator<IEnumerable<GoldHistoryDto>>>> GetLast7GoldHistory()
    {
        try
        {
            var goldHistoryList = await _goldHistoryService.GetLast7GoldHistoryAsync();
            if (goldHistoryList == null || !goldHistoryList.Any())
            {
                return NotFound(ResponseValidator<IEnumerable<GoldHistoryDto>>.Failure("No gold history records found."));
            }
            return Ok(ResponseValidator<IEnumerable<GoldHistoryDto>>.Success(goldHistoryList));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<IEnumerable<GoldHistoryDto>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<IEnumerable<GoldHistoryDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPost("Add", Name = "AddGoldHistory")]
    public async Task<ActionResult<ResponseValidator<GoldHistoryDto>>> AddGoldHistory()
    {
        try
        {
            var addedGoldHistory = await _goldHistoryService.AddGoldHistoryAsync();
            return Ok(ResponseValidator<GoldHistoryDto>.Success(addedGoldHistory));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<GoldHistoryDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(ResponseValidator<GoldHistoryDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<GoldHistoryDto>.Failure($"An error occurred: {e.Message}"));
        }
    }
}