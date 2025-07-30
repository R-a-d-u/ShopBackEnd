using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.SaleRecordValidation
{
    public class SaleRecordAddValidation : AbstractValidator<SaleRecordDtoAdd>
    {
        public SaleRecordAddValidation()
        {
            RuleFor(s => s.OrderItemId)
                .NotEmpty().WithMessage("OrderItemId is required.")
                .GreaterThan(0).WithMessage("OrderItemId must be a positive number.");

            RuleFor(s => s.OrderId)
               .NotEmpty().WithMessage("OrderId is required.");

            RuleFor(s => s.SaleDate)
                .NotEmpty().WithMessage("SaleDate is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("SaleDate cannot be in the future.");
        }
    }
}
