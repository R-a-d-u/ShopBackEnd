using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.CartValidation
{
    public class CartIdValidation : AbstractValidator<int>
    {
        public CartIdValidation()
        {
            RuleFor(id => id)
                .GreaterThan(0).WithMessage("Cart ID must be a positive integer.");
        }
    }

}
