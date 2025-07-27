namespace ShopBackEnd.Data.Mapper.ProductMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class ProductEditSellingPriceMapper
{
    
    public static void UpdateEntity(Product product, ProductDtoEditSellingPrice editPriceDto)
    {
        if (product == null || editPriceDto == null) return;

        product.SellingPrice = editPriceDto.AdditionalValue;
        product.LastModifiedDate = editPriceDto.LastModifiedDate;
    }
}
