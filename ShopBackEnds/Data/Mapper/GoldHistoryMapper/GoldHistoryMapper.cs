using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
namespace ShopBackEnd.Data.Mapper.GoldHistoryMapper;

public class GoldHistoryMapper
{
   
    public static GoldHistoryDto ToDto(GoldHistory goldHistory)
    {
        if (goldHistory == null) return null;
        return new GoldHistoryDto
        {
            Id = goldHistory.Id,
            Metal = goldHistory.Metal,
            PriceOunce = goldHistory.PriceOunce,
            PriceGram = goldHistory.PriceGram,
            PercentageChange = goldHistory.PercentageChange,
            Exchange = goldHistory.Exchange,
            Timestamp = goldHistory.Timestamp,
            Date = goldHistory.Date
        };
    }

   
    public static GoldHistory ToEntity(GoldHistoryDto goldHistoryDto)
    {
        if (goldHistoryDto == null) return null;
        return new GoldHistory
        {
            Id = goldHistoryDto.Id,
            Metal = goldHistoryDto.Metal,
            PriceOunce = goldHistoryDto.PriceOunce,
            PriceGram = goldHistoryDto.PriceGram,
            PercentageChange = goldHistoryDto.PercentageChange,
            Exchange = goldHistoryDto.Exchange,
            Timestamp = goldHistoryDto.Timestamp,
            Date = goldHistoryDto.Date
        };
    }
}