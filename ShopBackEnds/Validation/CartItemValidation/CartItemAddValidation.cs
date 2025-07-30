using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.CartItemValidation
{
    public class CartItemAddValidation : AbstractValidator<CartItemDtoAdd>
    {
        public CartItemAddValidation()
        {
            RuleFor(ci => ci.CartId)
                .GreaterThan(0).WithMessage("Cart ID must be a positive integer.");

            RuleFor(ci => ci.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be a positive integer.");

            RuleFor(ci => ci.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(ci => ci.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero.");

        }
    }
}
