using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.OrderValidation
{
    public class OrderQueryValidation : AbstractValidator<OrderDtoQuery>
    {
        public OrderQueryValidation()
        {
            RuleFor(o => o.FromDate)
                .LessThanOrEqualTo(o => o.ToDate)
                .When(o => o.FromDate.HasValue && o.ToDate.HasValue)
                .WithMessage("FromDate must be earlier than or equal to ToDate.");

            RuleFor(o => o.MinTotalSum)
                .LessThanOrEqualTo(o => o.MaxTotalSum)
                .When(o => o.MinTotalSum.HasValue && o.MaxTotalSum.HasValue)
                .WithMessage("MinTotalSum must be less than or equal to MaxTotalSum.");

            RuleFor(o => o.PaymentMethod)
                .IsInEnum()
                .When(o => o.PaymentMethod.HasValue)
                .WithMessage("PaymentMethod must be a valid value.");

            RuleFor(o => o.OrderStatus)
                .IsInEnum()
                .When(o => o.OrderStatus.HasValue)
                .WithMessage("OrderStatus must be a valid value.");
        }
    }

}
