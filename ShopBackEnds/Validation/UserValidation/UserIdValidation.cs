using FluentValidation;
using ShopBackEnd.Data.Dto;
namespace ShopBackEnd.Validation.UserValidation
{
    public class UserIdValidation : AbstractValidator<int>
    {
        public UserIdValidation()
        {
            RuleFor(id => id).GreaterThan(0).WithMessage("User Id must be greater than 0");
        }
    }
}
