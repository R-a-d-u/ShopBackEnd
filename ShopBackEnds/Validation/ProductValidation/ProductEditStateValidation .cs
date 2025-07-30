using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.ProductValidation
{
    public class ProductEditStateValidation : AbstractValidator<ProductDtoEditState>
    {
        public ProductEditStateValidation()
        {
        }
    }
}
