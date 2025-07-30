using FluentValidation;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.UserValidation
{
    public class UserEditValidation : AbstractValidator<UserDtoEdit>
    {
        public UserEditValidation()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(u => u.LastModifyDate)
                .NotEmpty().WithMessage("LastModifyDate is required.");
        }
    }

}
