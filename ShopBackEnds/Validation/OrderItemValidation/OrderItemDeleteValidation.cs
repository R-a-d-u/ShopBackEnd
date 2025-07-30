using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.OrderItemValidation
{
    public class OrderItemDeleteValidation : AbstractValidator<OrderItemDto>
    {
        public OrderItemDeleteValidation()
        {
            RuleFor(oi => oi.Id)
                .GreaterThan(0).WithMessage("Id must be a positive number.");

        }
    }
}
