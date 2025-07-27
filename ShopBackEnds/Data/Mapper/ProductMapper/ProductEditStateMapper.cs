namespace ShopBackEnd.Data.Mapper.ProductMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class ProductEditStateMapper
{
    
    public static void UpdateEntity(Product product, ProductDtoEditState editStateDto)
    {
        if (product == null || editStateDto == null) return;

        product.ProductState = editStateDto.ProductState;
        product.LastModifiedDate = editStateDto.LastModifiedDate;
    }
}
