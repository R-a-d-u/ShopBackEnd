using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.OrderItemValidation
{
    public class OrderItemEditValidation : AbstractValidator<OrderItemEditDto>
    {
        public OrderItemEditValidation()
        {
            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(oi => oi.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero.");
        }
    }
}
