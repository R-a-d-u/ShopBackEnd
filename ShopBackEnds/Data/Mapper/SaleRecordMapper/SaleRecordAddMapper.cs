namespace ShopBackEnd.Data.Mapper.SaleRecordMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class SaleRecordAddMapper
{
    
    public static SaleRecord ToEntity(SaleRecordDtoAdd saleRecordDtoAdd)
    {
        if (saleRecordDtoAdd == null) return null;

        return new SaleRecord
        {
            OrderItemId = saleRecordDtoAdd.OrderItemId,
            OrderId = saleRecordDtoAdd.OrderId,
            SaleDate = saleRecordDtoAdd.SaleDate,
        };
    }
}