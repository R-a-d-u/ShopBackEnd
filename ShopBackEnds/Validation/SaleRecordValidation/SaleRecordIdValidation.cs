using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.SaleRecordValidation
{
    public class SaleRecordIdValidation : AbstractValidator<int>
    {
        public SaleRecordIdValidation()
        {
            RuleFor(id => id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be a positive number.");
        }
    }
}
