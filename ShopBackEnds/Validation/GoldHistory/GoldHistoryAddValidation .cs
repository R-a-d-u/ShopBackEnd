using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.GoldHistory
{
    public class GoldHistoryAddValidation : AbstractValidator<GoldHistoryDtoAdd>
    {
        public GoldHistoryAddValidation()
        {
            RuleFor(x => x.Metal)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Metal name is required and cannot exceed 255 characters.");

            RuleFor(x => x.PriceOunce)
                .GreaterThan(0)
                .WithMessage("Price per ounce must be greater than 0.");

            RuleFor(x => x.PriceGram)
                .GreaterThan(0)
                .WithMessage("Price per gram must be greater than 0.");

            RuleFor(x => x.Exchange)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Exchange name is required and cannot exceed 255 characters.");

            RuleFor(x => x.Timestamp)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Timestamp is required and cannot exceed 255 characters.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");
        }
    }
}
