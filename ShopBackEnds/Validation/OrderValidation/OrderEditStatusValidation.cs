using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Validations
{
    public class OrderEditStatusValidation : AbstractValidator<OrderDto>
    {
        public OrderEditStatusValidation()
        {
            RuleFor(o => o.Id)
                .NotNull().NotEmpty();

            RuleFor(o => o.OrderStatus)
                .IsInEnum().WithMessage("OrderStatus must be a valid value.")
                .NotNull().WithMessage("OrderStatus is required.")
                .Must(status => Enum.IsDefined(typeof(OrderStatus), status))
                .WithMessage("Invalid OrderStatus value.");
        }
    }
}
