using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Services
{
    public interface ISaleRecordService
    {
        Task<List<ProductSalesSummaryDto>> GetProductSalesSummaryBetweenDates(DateTime startDate, DateTime endDate);
        Task<List<HourlySalesSummaryDto>> GetHourlySalesSummary(DateTime startDate, DateTime endDate);
        Task<RevenueAnalysisDto> GetRevenueAnalysis(DateTime startDate, DateTime endDate);
        Task<CustomerPurchasePatternDto> GetCustomerPurchasePatterns(int userId);
        Task<List<CategorySalesDto>> GetCategorySalesPerformance(DateTime startDate, DateTime endDate);
    }
}
