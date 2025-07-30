using FluentValidation;

namespace ShopBackEnd.Validation.ProductValidation
{
    public class ProductIdValidation : AbstractValidator<int>
    {
        public ProductIdValidation() {
            RuleFor(id => id)
                   .GreaterThan(0)
                   .WithMessage("Gold history ID must be greater than 0.");
        }
       
    }
}
