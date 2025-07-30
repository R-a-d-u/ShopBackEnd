using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.ProductValidation
{
    public class ProductFilterValidation : AbstractValidator<ProductDtoFilter>
    {
        public ProductFilterValidation()
        {
            RuleFor(p => p.Name)
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(p => p.MinPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum price must be non-negative.");

            RuleFor(p => p.MaxPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum price must be non-negative.")
                .GreaterThanOrEqualTo(p => p.MinPrice).WithMessage("Maximum price must be greater than or equal to minimum price.");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).When(p => p.CategoryId.HasValue).WithMessage("CategoryId must be a positive number.");

            RuleFor(p => p.ProductState)
                .IsInEnum().When(p => p.ProductState.HasValue).WithMessage("Invalid product state value.");
        }
    }
}
