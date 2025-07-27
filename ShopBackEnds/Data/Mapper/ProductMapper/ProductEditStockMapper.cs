namespace ShopBackEnd.Data.Mapper.ProductMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class ProductEditStockMapper
{
    
    public static void UpdateEntity(Product product, ProductDtoEditStock editStockDto)
    {
        if (product == null || editStockDto == null) return;

        product.StockQuantity = editStockDto.StockQuantity; // Renamed from Stock
        product.LastModifiedDate = editStockDto.LastModifiedDate;
    }
}
