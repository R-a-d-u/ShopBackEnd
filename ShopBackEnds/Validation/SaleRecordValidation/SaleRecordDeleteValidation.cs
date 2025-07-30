using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.SaleRecordValidation
{
    public class SaleRecordDeleteValidation : AbstractValidator<int>
    {
        public SaleRecordDeleteValidation()
        {
            RuleFor(id => id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be a positive number.");
        }
    }
}
