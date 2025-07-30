using FluentValidation;

namespace ShopBackEnd.Validation.GoldHistory
{
    public class GoldHistoryIdValidation : AbstractValidator<int>
    {
        public GoldHistoryIdValidation()
        {
            RuleFor(id => id)
                .GreaterThan(0)
                .WithMessage("Gold history ID must be greater than 0.");
        }
    }
}
