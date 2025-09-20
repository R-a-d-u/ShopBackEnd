using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public interface ISaleRecordRepository
    {
        Task<List<ProductSalesSummaryDto>> GetProductSalesSummaryBetweenDates(DateTime startDate, DateTime endDate);
        Task<List<HourlySalesSummaryDto>> GetHourlySalesSummary(DateTime startDate, DateTime endDate);
        Task<RevenueAnalysisDto> GetRevenueAnalysis(DateTime startDate, DateTime endDate);
        Task<List<CategorySalesDto>> GetCategorySalesPerformance(DateTime startDate, DateTime endDate);
        Task<CustomerPurchasePatternDto> GetCustomerPurchasePattern(int userId);
    }
}
