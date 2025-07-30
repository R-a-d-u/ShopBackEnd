using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Validation.ProductValidation
{
    public class ProductEditStockValidation : AbstractValidator<ProductDtoEditStock>
    {
        public ProductEditStockValidation()
        {
            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be a non-negative number.");

        }
    }
}
