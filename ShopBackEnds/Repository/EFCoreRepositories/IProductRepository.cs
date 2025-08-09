using ShopBackEnd.Data.Dto;
using ShopBackEnd.HelperClass;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public interface IProductRepository
    {
        Task<ProductDto?> GetProductById(int id);
        Task<PagedResult<ProductDto>> GetAllProductsByCategoryId(int categoryId, int pageNumber, int pageSize);
        Task<PagedResult<ProductDto>> GetAllDiscontinuedProducts(int pageNumber, int pageSize);
        Task<PagedResult<ProductDto>> GetAllProductsByName(string name, int pageNumber, int pageSize);
        Task<ProductDtoEditInformation?> GetProductInformationById(int id);
        Task<bool> SetStateToOutOfStock(int id, ProductDtoEditState dto);
        Task<bool> SetStateToDiscontinued(int id, ProductDtoEditState dto);
        Task<bool> SetStateToInStock(int id, ProductDtoEditState dto);
        Task<bool> EditProductStockQuantity(int id, ProductDtoEditStock dto);
        Task<bool> EditProductAdditionalPrice(int id, ProductDtoEditSellingPrice dto);
        Task<bool> EditProductInformation(int id, ProductDtoEditInformation dto);
        Task<int> AddProduct(ProductDtoAdd dto);
        Task<bool> UpdateAllGoldProductPrices();
        Task<List<ProductDto>> FilterAllProductsByCategory(int categoryId);
        Task<List<ProductDto>> FilterAllProducts(ProductDtoFilter filter);
        Task<int> GetTotalNonDiscontinuedProductsCount();
        Task<int> GetTotalLowStockProductsCount();
    }
}
