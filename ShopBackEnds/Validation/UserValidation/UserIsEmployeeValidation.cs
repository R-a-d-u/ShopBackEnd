using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Enums;
namespace ShopBackEnd.Validation.UserValidation
{
    public class UserIsEmployeeValidaton : AbstractValidator<UserDto>
    {
        public UserIsEmployeeValidaton()
        {
            RuleFor(u => u.UserAccessType)
                .Equal(UserAccesType.Employee).WithMessage("User must be an Employee.");
        }
    }
}
