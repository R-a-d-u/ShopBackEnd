using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Repository;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Service;
using ShopBackEnd.Validation.GoldHistory;

namespace ShopBackEnd.Service
{
    public class GoldHistoryService : IGoldHistoryService
    {
        private readonly IGoldHistoryRepository _goldHistoryRepository;
        private readonly GoldHistoryIdValidation _goldHistoryIdValidator;
        private readonly GoldHistoryAddValidation _addGoldHistoryValidator;

        public GoldHistoryService(
            IGoldHistoryRepository goldHistoryRepository,
            GoldHistoryIdValidation goldHistoryIdValidator,
            GoldHistoryAddValidation addGoldHistoryValidator)
        {
            _goldHistoryRepository = goldHistoryRepository;
            _goldHistoryIdValidator = goldHistoryIdValidator;
            _addGoldHistoryValidator = addGoldHistoryValidator;
        }

        public async Task<GoldHistoryDto> GetGoldHistoryByIdAsync(int id)
        {
            var validationResult = _goldHistoryIdValidator.Validate(id);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var goldHistory = await _goldHistoryRepository.GetGoldHistoryByIdAsync(id);
            if (goldHistory == null)
            {
                throw new KeyNotFoundException($"Gold history with ID {id} not found.");
            }

            return goldHistory;
        }
        public async Task<List<GoldHistoryDto>> GetGoldPricesBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            return await _goldHistoryRepository.GetGoldPricesBetweenDatesAsync(startDate, endDate);
        }
        public async Task<GoldHistoryDto> GetLastGoldPriceHistoryAsync()
        {
            return await _goldHistoryRepository.GetLastGoldPriceHistoryAsync();
        }

        public async Task<decimal> GetLastPriceInGramsAsync()
        {
            return await _goldHistoryRepository.GetLastPriceInGramsAsync();
        }

        public async Task<List<GoldHistoryDto>> GetLast7GoldHistoryAsync()
        {
            return await _goldHistoryRepository.GetLast7GoldHistoryAsync();
        }

        public async Task<GoldHistoryDto> AddGoldHistoryAsync()
        {
            return await _goldHistoryRepository.AddGoldHistoryAsync();
        }
    }
}