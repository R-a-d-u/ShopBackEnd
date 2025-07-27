using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
namespace ShopBackEnd.Data.Mapper.GoldHistoryMapper;

public class GoldHistoryAddMapper
{
    public static GoldHistoryDtoAdd ToDto(GoldHistory goldHistory)
    {
        if (goldHistory == null) return null;
        return new GoldHistoryDtoAdd
        {
            Metal = goldHistory.Metal,
            PriceOunce = goldHistory.PriceOunce,
            PriceGram = goldHistory.PriceGram,
            PercentageChange = goldHistory.PercentageChange,
            Exchange = goldHistory.Exchange,
            Timestamp = goldHistory.Timestamp,
            Date = goldHistory.Date
        };
    }

    public static GoldHistory ToEntity(GoldHistoryDtoAdd goldHistoryDtoAdd)
    {
        if (goldHistoryDtoAdd == null) return null;
        return new GoldHistory
        {
            Metal = goldHistoryDtoAdd.Metal,
            PriceOunce = goldHistoryDtoAdd.PriceOunce,
            PriceGram = goldHistoryDtoAdd.PriceGram,
            PercentageChange = goldHistoryDtoAdd.PercentageChange,
            Exchange = goldHistoryDtoAdd.Exchange,
            Timestamp = goldHistoryDtoAdd.Timestamp,
            Date = goldHistoryDtoAdd.Date
        };
    }
}