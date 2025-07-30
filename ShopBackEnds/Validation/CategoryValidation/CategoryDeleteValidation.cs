using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.CategoryValidation
{
    public class CategoryDeleteValidation : AbstractValidator<CategoryDto>
    {
        public CategoryDeleteValidation()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Category Id must be a positive number.");
        }
    }
}
