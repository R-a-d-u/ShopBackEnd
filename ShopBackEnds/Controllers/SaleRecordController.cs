using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Services;
using ShopBackEnd.Validators.ResponseValidator;

[ApiController]
[Route("[controller]")]
public class SaleRecordsController : ControllerBase
{
    private readonly ISaleRecordService _saleRecordService;

    public SaleRecordsController(ISaleRecordService saleRecordService)
    {
        _saleRecordService = saleRecordService;
    }
    [HttpGet("GetProductSalesSummary", Name = "GetProductSalesSummary")]
    public async Task<ActionResult<ResponseValidator<List<ProductSalesSummaryDto>>>> GetProductSalesSummary(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var salesSummary = await _saleRecordService.GetProductSalesSummaryBetweenDates(startDate, endDate);

            if (salesSummary == null || salesSummary.Count == 0)
            {
                return NotFound(ResponseValidator<List<ProductSalesSummaryDto>>.Failure("No sales records found for the specified date range."));
            }

            return Ok(ResponseValidator<List<ProductSalesSummaryDto>>.Success(salesSummary));
        }
        catch (ArgumentException e)
        {
            return BadRequest(ResponseValidator<List<ProductSalesSummaryDto>>.Failure(e.Message));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<List<ProductSalesSummaryDto>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<List<ProductSalesSummaryDto>>.Failure($"An error occurred: {e.Message}"));
        }

    }

    [HttpGet("GetHourlySalesSummary", Name = "GetHourlySalesSummary")]
    public async Task<ActionResult<ResponseValidator<List<HourlySalesSummaryDto>>>> GetHourlySalesSummary(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var hourlySalesSummary = await _saleRecordService.GetHourlySalesSummary(startDate, endDate);

            if (hourlySalesSummary == null || hourlySalesSummary.Count == 0)
            {
                return NotFound(ResponseValidator<List<HourlySalesSummaryDto>>.Failure("No sales records found for the specified date range."));
            }

            return Ok(ResponseValidator<List<HourlySalesSummaryDto>>.Success(hourlySalesSummary));
        }
        catch (ArgumentException e)
        {
            return BadRequest(ResponseValidator<List<HourlySalesSummaryDto>>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<List<HourlySalesSummaryDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("GetRevenueAnalysis", Name = "GetRevenueAnalysis")]
    public async Task<ActionResult<ResponseValidator<RevenueAnalysisDto>>> GetRevenueAnalysis(
    [FromQuery] DateTime startDate,
    [FromQuery] DateTime endDate)
    {
        try
        {
            var revenueAnalysis = await _saleRecordService.GetRevenueAnalysis(startDate, endDate);

            if (revenueAnalysis == null)
            {
                return NotFound(ResponseValidator<RevenueAnalysisDto>.Failure("No revenue data found for the specified date range."));
            }

            return Ok(ResponseValidator<RevenueAnalysisDto>.Success(revenueAnalysis));
        }
        catch (ArgumentException e)
        {
            return BadRequest(ResponseValidator<RevenueAnalysisDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<RevenueAnalysisDto>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("GetCategorySalesPerformance", Name = "GetCategorySalesPerformance")]
    public async Task<ActionResult<ResponseValidator<List<CategorySalesDto>>>> GetCategorySalesPerformance(
      [FromQuery] DateTime startDate,
      [FromQuery] DateTime endDate)
    {
        try
        {
            var categorySales = await _saleRecordService.GetCategorySalesPerformance(startDate, endDate);
            return Ok(ResponseValidator<List<CategorySalesDto>>.Success(categorySales));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<List<CategorySalesDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetCustomerPurchasePatterns", Name = "GetCustomerPurchasePatterns")]
    public async Task<ActionResult<ResponseValidator<CustomerPurchasePatternDto>>> GetCustomerPurchasePatterns(
     [FromQuery] int userId)
    {
        try
        {
            var customerPatterns = await _saleRecordService.GetCustomerPurchasePatterns(userId);

            if (customerPatterns == null)
            {
                return NotFound(ResponseValidator<CustomerPurchasePatternDto>.Failure("No purchase data found for this user."));
            }

            return Ok(ResponseValidator<CustomerPurchasePatternDto>.Success(customerPatterns));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<CustomerPurchasePatternDto>.Failure("Validation error: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<CustomerPurchasePatternDto>.Failure($"An error occurred: {e.Message}"));
        }
    }


}