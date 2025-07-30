using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validations
{
    public class OrderIdValidation : AbstractValidator<Guid>
    {
        public OrderIdValidation()
        {
            RuleFor(Id => Id)
                .NotNull().NotEmpty();
        }
    }
}
