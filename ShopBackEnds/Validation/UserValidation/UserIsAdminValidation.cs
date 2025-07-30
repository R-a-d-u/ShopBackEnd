using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Data.Enums;
namespace ShopBackEnd.Validation.UserValidation
{
    public class UserIsAdminValidaton : AbstractValidator<UserDto>
    {
        public UserIsAdminValidaton()
        {
            RuleFor(u => u.UserAccessType)
                .Equal(UserAccesType.Admin).WithMessage("User must be an Admin.");
        }
    }
}
