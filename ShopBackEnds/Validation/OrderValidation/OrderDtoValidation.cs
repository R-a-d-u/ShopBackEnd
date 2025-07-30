using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.OrderValidation
{
    public class OrderDtoValidation : AbstractValidator<OrderDto>
    {
        public OrderDtoValidation()
        {
            RuleFor(o => o.Id)
                .NotNull().NotEmpty();

            RuleFor(o => o.UserId)
                .GreaterThan(0).WithMessage("UserId must be a positive number.");

            RuleFor(o => o.OrderCreatedDate)
                .NotEmpty().WithMessage("OrderCreatedDate is required.");

            RuleFor(o => o.TotalSum)
                .GreaterThan(0).WithMessage("TotalSum must be greater than 0.");

            RuleFor(o => o.ShippingFee)
                .GreaterThanOrEqualTo(0).WithMessage("ShippingFee cannot be negative.");

            RuleFor(o => o.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

            RuleFor(o => o.PaymentMethod)
                .IsInEnum().WithMessage("PaymentMethod must be a valid value.")
                .NotNull().WithMessage("PaymentMethod is required.");

            RuleFor(o => o.OrderStatus)
                .IsInEnum().WithMessage("OrderStatus must be a valid value.")
                .NotNull().WithMessage("OrderStatus is required.");

        }
    }

}
