using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.ProductValidation
{
    public class ProductEditInformationValidation : AbstractValidator<ProductDtoEditInformation>
    {
        public ProductEditInformationValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be a positive number.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        }
    }
}
