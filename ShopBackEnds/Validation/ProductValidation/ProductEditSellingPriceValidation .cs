using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.ProductValidation
{
    public class ProductEditSellingPriceValidation : AbstractValidator<ProductDtoEditSellingPrice>
    {
        public ProductEditSellingPriceValidation()
        {
            RuleFor(p => p.AdditionalValue)
                .GreaterThanOrEqualTo(0).WithMessage("Selling price must be non-negative.");

        }
    }
}
