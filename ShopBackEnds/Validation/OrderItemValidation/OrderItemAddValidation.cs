using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.OrderItemValidation
{
    public class OrderItemAddValidation : AbstractValidator<OrderItemDtoAdd>
    {
        public OrderItemAddValidation()
        {
            RuleFor(oi => oi.OrderId)
                .NotNull().NotEmpty();

            RuleFor(oi => oi.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be a positive number.");

            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(oi => oi.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero.");
        }
    }
}
