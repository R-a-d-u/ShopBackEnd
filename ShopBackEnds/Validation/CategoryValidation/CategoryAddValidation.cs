using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.Validation.UserValidation;

namespace ShopBackEnd.Validation.CategoryValidation
{
    public class CategoryAddValidation : AbstractValidator<CategoryDtoAdd>
    {
        public CategoryAddValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category Name is required.")
                .MaximumLength(255).WithMessage("Category Name cannot exceed 255 characters.");

        }
    }
}
    
