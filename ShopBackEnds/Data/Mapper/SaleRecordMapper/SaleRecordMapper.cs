
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
namespace ShopBackEnd.Data.Mapper.SaleRecordMapper;
public class SaleRecordMapper
{
    
    public static SaleRecordDto ToDto(SaleRecord saleRecord)
    {
        if (saleRecord == null) return null;

        return new SaleRecordDto
        {
            Id = saleRecord.Id,
            OrderItemId = saleRecord.OrderItemId,
            OrderId = saleRecord.OrderId,
            SaleDate = saleRecord.SaleDate,
        };
    }

    
    public static SaleRecord ToEntity(SaleRecordDto saleRecordDto)
    {
        if (saleRecordDto == null) return null;

        return new SaleRecord
        {
            Id = saleRecordDto.Id,
            OrderItemId = saleRecordDto.OrderItemId,
            OrderId = saleRecordDto.OrderId,
            SaleDate = saleRecordDto.SaleDate,
        };
    }
}