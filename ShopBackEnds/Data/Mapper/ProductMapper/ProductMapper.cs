namespace ShopBackEnd.Data.Mapper.ProductMapper;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;

public class ProductMapper
{
    
    public static ProductDto ToDto(Product product)
    {
        if (product == null) return null;

        return new ProductDto
        {
            Id = product.Id,
            ProductType = product.ProductType, 
            Name = product.Name,
            Image = product.Image,
            AdditionalValue = product.AdditionalValue, 
            GoldWeightInGrams = product.GoldWeightInGrams, 
            SellingPrice = product.SellingPrice,
            CategoryId = product.CategoryId,
            Description = product.Description,
            StockQuantity = product.StockQuantity, 
            ProductState = product.ProductState,
            LastModifiedDate = product.LastModifiedDate
        };
    }

   
    public static Product ToEntity(ProductDto productDto)
    {
        if (productDto == null) return null;

        return new Product
        {
            Id = productDto.Id,
            ProductType = productDto.ProductType, 
            Name = productDto.Name,
            Image = productDto.Image,
            AdditionalValue = productDto.AdditionalValue, 
            GoldWeightInGrams = productDto.GoldWeightInGrams, 
            SellingPrice = productDto.SellingPrice,
            CategoryId = productDto.CategoryId,
            Description = productDto.Description,
            StockQuantity = productDto.StockQuantity, 
            ProductState = productDto.ProductState,
            LastModifiedDate = productDto.LastModifiedDate
        };
    }
}