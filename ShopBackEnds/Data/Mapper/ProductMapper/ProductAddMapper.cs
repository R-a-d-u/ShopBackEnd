namespace ShopBackEnd.Data.Mapper.ProductMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class ProductAddMapper
{
    
    public static Product ToEntity(ProductDtoAdd productDtoAdd)
    {
        if (productDtoAdd == null) return null;

        return new Product
        {
            ProductType = productDtoAdd.ProductType, 
            Name = productDtoAdd.Name,
            Image = productDtoAdd.Image,
            AdditionalValue = productDtoAdd.AdditionalValue, 
            GoldWeightInGrams = productDtoAdd.GoldWeightInGrams, 
            SellingPrice = productDtoAdd.SellingPrice,
            CategoryId = productDtoAdd.CategoryId,
            Description = productDtoAdd.Description,
            StockQuantity = productDtoAdd.StockQuantity,
            ProductState = productDtoAdd.ProductState,
            LastModifiedDate = productDtoAdd.LastModifiedDate
        };
    }
}
