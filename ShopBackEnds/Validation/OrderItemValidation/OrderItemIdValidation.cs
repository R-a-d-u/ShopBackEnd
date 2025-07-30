using FluentValidation;
namespace ShopBackEnd.Validation.OrderItemValidation
{
    public class OrderItemIdValidation : AbstractValidator<int>
    {
        public OrderItemIdValidation()  
        {
            RuleFor(id => id)
                .GreaterThan(0).WithMessage("ID must be a positive number.");
        }
    }
}
