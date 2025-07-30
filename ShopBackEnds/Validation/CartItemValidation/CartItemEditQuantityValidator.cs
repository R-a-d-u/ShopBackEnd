using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.CartItemValidation
{
    public class CartItemEditQuantityValidator : AbstractValidator<int>
    {
        public CartItemEditQuantityValidator() 
        {
            RuleFor(Quantity => Quantity)
               .GreaterThan(-1).WithMessage("Quantity must be a positive number.");
        }
    }
}
