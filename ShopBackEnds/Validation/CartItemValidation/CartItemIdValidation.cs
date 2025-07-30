using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.CartItemValidation
{
    public class CartItemIdValidation : AbstractValidator<int>
    {
        public CartItemIdValidation()
        {
            RuleFor(id => id)
                .GreaterThan(0).WithMessage("Cart Item ID must be a positive integer.");
        }
    }
}
