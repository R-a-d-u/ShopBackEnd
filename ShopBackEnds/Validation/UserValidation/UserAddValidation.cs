using FluentValidation;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.UserValidation
{
    public class UserAddValidation : AbstractValidator<UserDtoAdd>
    {
        public UserAddValidation()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(255).WithMessage("Password cannot exceed 255 characters.");

            RuleFor(u => u.UserAccessType)
                .IsInEnum().WithMessage("Invalid access type.");

            RuleFor(u => u.LastModifyDate)
                .NotEmpty().WithMessage("LastModifyDate is required.");
        }
    }
}