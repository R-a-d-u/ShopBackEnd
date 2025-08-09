using ShopBackEnd.Data.Entity;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Validation.CartItemValidation;
using FluentValidation;
using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly CartItemIdValidation _cartItemIdValidator;
        private readonly CartItemEditQuantityValidator _cartItemEditQuantityValidator;
        private readonly ILogger<CartItemService> _logger;

        public CartItemService(
            ICartItemRepository cartItemRepository,
            CartItemIdValidation cartItemIdValidator,
            CartItemEditQuantityValidator cartItemEditQuantityValidator,
            ILogger<CartItemService> logger)
        {
            _cartItemRepository = cartItemRepository;
            _cartItemIdValidator = cartItemIdValidator;
            _cartItemEditQuantityValidator= cartItemEditQuantityValidator;
            _logger = logger;
        }

        public async Task<CartItemDto> GetCartItemById(int cartItemId)
        {
            var validationResult = _cartItemIdValidator.Validate(cartItemId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _cartItemRepository.GetCartItemById(cartItemId);
        }

        public async Task<CartItemDto> UpdateCartItemQuantity(int cartItemId, int newQuantity)
        {
            var validationResult = _cartItemIdValidator.Validate(cartItemId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var validationResultQuantity = _cartItemEditQuantityValidator.Validate(newQuantity);
            if (!validationResultQuantity.IsValid)
            {
                throw new ValidationException(validationResultQuantity.Errors);
            }

            return await _cartItemRepository.UpdateCartItemQuantity(cartItemId, newQuantity);
        }
    }
}

