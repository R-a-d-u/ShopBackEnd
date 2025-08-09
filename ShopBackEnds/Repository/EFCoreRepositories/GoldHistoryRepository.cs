using Microsoft.EntityFrameworkCore;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Repository.Context;
using ShopBackEnd.Data.Mapper.GoldHistoryMapper;
using ShopBackEnd.Service;
using ShopBackEnd.HelperClass;
using System.Text.Json;
using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class GoldHistoryRepository : IGoldHistoryRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<GoldHistoryService> _logger;
        private readonly GoldApiFetch _apiFetch;
        private readonly IGoldPriceUpdateAllProductsRepository _goldPriceUpdateAllProductsRepository;

        public GoldHistoryRepository(ShopDbContext context, ILogger<GoldHistoryService> logger, GoldApiFetch apiFetch, IGoldPriceUpdateAllProductsRepository goldPriceUpdateAllProductsRepository)
        {
            _context = context;
            _logger = logger;
            _apiFetch = apiFetch;
            _goldPriceUpdateAllProductsRepository = goldPriceUpdateAllProductsRepository;
        }

        public async Task<GoldHistoryDto> GetGoldHistoryByIdAsync(int id)
        {
            var goldHistory = await _context.GoldHistory.FindAsync(id);

            if (goldHistory == null)
            {
                throw new InvalidOperationException("Gold history id not found.");
            }

            return GoldHistoryMapper.ToDto(goldHistory);
        }

        public async Task<List<GoldHistoryDto>> GetLast7GoldHistoryAsync()
        {
            var goldHistories = await _context.GoldHistory
                .OrderByDescending(g => g.Date)
                .Take(7)
                .ToListAsync();

            return goldHistories.Select(GoldHistoryMapper.ToDto).ToList();
        }
        public async Task<List<GoldHistoryDto>> GetGoldPricesBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            var goldPrices = await _context.GoldHistory
                .Where(g => g.Date >= startDate && g.Date <= endDate)
                .OrderByDescending(g => g.Date) 
                .ToListAsync();

            return goldPrices.Select(GoldHistoryMapper.ToDto).ToList();
        }
        public async Task<GoldHistoryDto> GetLastGoldPriceHistoryAsync()
        {
            var latestGoldHistory = await _context.GoldHistory
                .OrderByDescending(g => g.Date)
                .FirstOrDefaultAsync();

            if (latestGoldHistory == null)
            {
                throw new InvalidOperationException("No gold history found.");
            }

            return GoldHistoryMapper.ToDto(latestGoldHistory);
        }

        public async Task<decimal> GetLastPriceInGramsAsync()
        {
            var latestGoldHistory = await _context.GoldHistory
                .OrderByDescending(g => g.Date)
                .FirstOrDefaultAsync();

            if (latestGoldHistory == null)
            {
                throw new InvalidOperationException("No gold history found.");
            }

            return latestGoldHistory.PriceGram;
        }

        public async Task<GoldHistoryDto> AddGoldHistoryAsync()
        {
            var latestRecord = await _context.GoldHistory
                .OrderByDescending(g => g.Date)
                .FirstOrDefaultAsync();
            if (latestRecord != null && latestRecord.Date.Date == DateTime.Today)
            {
                throw new InvalidOperationException("A gold history record already exists for today's date.");
            }

            var goldPriceDataJson = await _apiFetch.GetGoldPriceFilteredAsync();

            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimestampConverter());

            var goldPriceData = JsonSerializer.Deserialize<GoldHistoryDtoAdd>(goldPriceDataJson, options);
            if (goldPriceData == null)
            {
                throw new InvalidOperationException("Unable to fetch gold price data.");
            }

            var goldHistoryDtoAdd = new GoldHistoryDtoAdd
            {
                Metal = goldPriceData.Metal,
                PriceOunce = goldPriceData.PriceOunce,
                PriceGram = goldPriceData.PriceGram,
                PercentageChange = goldPriceData.PercentageChange,
                Exchange = goldPriceData.Exchange,
                Timestamp = goldPriceData.Timestamp,
                Date = DateTime.Today
            };

            var goldHistory = GoldHistoryAddMapper.ToEntity(goldHistoryDtoAdd);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.GoldHistory.Add(goldHistory);
                await _context.SaveChangesAsync();

                decimal latestGoldPriceInGrams = goldHistory.PriceGram;

                await _goldPriceUpdateAllProductsRepository.UpdateProductPricesBasedOnGoldPrice(latestGoldPriceInGrams);
                await _goldPriceUpdateAllProductsRepository.ClearAllCarts();

                // Commit the transaction
                await transaction.CommitAsync();

                return GoldHistoryMapper.ToDto(goldHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update gold prices");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}