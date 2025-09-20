using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Validation.UserValidation;

namespace ShopBackEnd.Services
{
    public class SaleRecordService : ISaleRecordService
    {
        private readonly ISaleRecordRepository _saleRecordRepository;
        private readonly UserIdValidation _userIdValidation;

        public SaleRecordService(ISaleRecordRepository saleRecordRepository,UserIdValidation userIdValidation)
        {
            _saleRecordRepository = saleRecordRepository;
            _userIdValidation= userIdValidation;
        }

        public async Task<List<ProductSalesSummaryDto>> GetProductSalesSummaryBetweenDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be before or equal to end date.");
            }

            return await _saleRecordRepository.GetProductSalesSummaryBetweenDates(startDate, endDate);
        }
        public async Task<List<HourlySalesSummaryDto>> GetHourlySalesSummary(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be before or equal to end date.");
            }

            return await _saleRecordRepository.GetHourlySalesSummary(startDate, endDate);
        }
        public async Task<RevenueAnalysisDto> GetRevenueAnalysis(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be before or equal to end date.");
            }

            return await _saleRecordRepository.GetRevenueAnalysis(startDate, endDate);
        }
        public async Task<CustomerPurchasePatternDto> GetCustomerPurchasePatterns(int userId)
        {
            var validationResult = _userIdValidation.Validate(userId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await _saleRecordRepository.GetCustomerPurchasePattern(userId);
        }
        public async Task<List<CategorySalesDto>> GetCategorySalesPerformance(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be before or equal to end date.");
            }

            return await _saleRecordRepository.GetCategorySalesPerformance(startDate, endDate);
        }
    }
}
