using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Validation.ProductValidation
{
    public class ProductDtoAddValidation : AbstractValidator<ProductDtoAdd>
    {
        public ProductDtoAddValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(p => p.ProductType)
                .IsInEnum().WithMessage("Invalid product type.") // Ensures valid ProductType is chosen
                .NotEmpty().WithMessage("Product type is required.");

            RuleFor(p => p.AdditionalValue)
                .GreaterThanOrEqualTo(0).WithMessage("Additional value must be non-negative.");

            RuleFor(p => p.GoldWeightInGrams)
                .GreaterThanOrEqualTo(0).WithMessage("Gold weight in grams must be non-negative.");

            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.")
                .GreaterThan(0).WithMessage("CategoryId must be a positive number.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be a non-negative number.");

        }
    }
}
