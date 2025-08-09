using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Service
{
    public interface IGoldHistoryService
    {
        Task<GoldHistoryDto> GetGoldHistoryByIdAsync(int id);
        Task<List<GoldHistoryDto>> GetGoldPricesBetweenDatesAsync(DateTime startDate, DateTime endDate);
        Task<List<GoldHistoryDto>> GetLast7GoldHistoryAsync();
        Task<GoldHistoryDto> GetLastGoldPriceHistoryAsync();
        Task<decimal> GetLastPriceInGramsAsync();
        Task<GoldHistoryDto> AddGoldHistoryAsync();
    }
}