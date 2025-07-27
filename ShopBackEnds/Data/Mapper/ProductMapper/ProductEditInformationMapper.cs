namespace ShopBackEnd.Data.Mapper.ProductMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class ProductEditInformationMapper
{
   
    public static void UpdateEntity(Product product, ProductDtoEditInformation editInfoDto)
    {
        if (product == null || editInfoDto == null) return;

        product.Name = editInfoDto.Name;
        product.ProductType = editInfoDto.ProductType;
        product.Image = editInfoDto.Image;
        product.CategoryId = editInfoDto.CategoryId;
        product.Description = editInfoDto.Description;
        product.LastModifiedDate = editInfoDto.LastModifiedDate;
    }
}
