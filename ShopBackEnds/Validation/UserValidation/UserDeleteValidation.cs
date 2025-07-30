using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
namespace ShopBackEnd.Validation.UserValidation

{
    public class UserDeleteValidation : AbstractValidator<UserDto>
    {
        public UserDeleteValidation()
        {
            RuleFor(Id => Id)
                .NotEmpty().WithMessage("User ID is required.");
        }

    }
}
