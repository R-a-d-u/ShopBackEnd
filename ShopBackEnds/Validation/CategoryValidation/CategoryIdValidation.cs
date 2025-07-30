using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.CategoryValidation
{
    public class CategoryIdValidation : AbstractValidator<int>
    {
        public CategoryIdValidation()
        {
            RuleFor(id => id)
                .GreaterThan(0).WithMessage("Category Id must be a positive number.");
        }
    }
}
