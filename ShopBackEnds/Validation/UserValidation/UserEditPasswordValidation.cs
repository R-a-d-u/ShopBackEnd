using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.UserValidation
{
    public class UserEditPasswordValidation : AbstractValidator<UserDtoEditPassword>
    {
        public UserEditPasswordValidation() 
        {
            RuleFor(u => u.Password)
               .MaximumLength(255).WithMessage("Password cannot exceed 255 characters.")
               .When(u => !string.IsNullOrEmpty(u.Password));

            RuleFor(u => u.LastModifyDate)
              .NotEmpty().WithMessage("LastModifyDate is required.");
        }
    }
}
