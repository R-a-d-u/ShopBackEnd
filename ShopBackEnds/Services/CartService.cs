// CartService implementation
using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Services;
using ShopBackEnd.Validation.CartItemValidation;
using ShopBackEnd.Validation.CartValidation;

namespace ShopBackEnd.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CartService> _logger;
        private readonly CartIdValidation _cartIdValidator;
        private readonly CartItemIdValidation _cartItemIdValidator;
        private readonly CartItemAddValidation _cartItemAddValidator;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ILogger<CartService> logger,
            CartIdValidation cartIdValidator,
            CartItemIdValidation cartItemIdValidator,
            CartItemAddValidation cartItemAddValidator)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _logger = logger;
            _cartIdValidator = cartIdValidator;
            _cartItemIdValidator = cartItemIdValidator;
            _cartItemAddValidator = cartItemAddValidator;
        }

        public async Task<List<CartItemDto>> GetAllCartItems(int cartId)
        {
            var validationResult = _cartIdValidator.Validate(cartId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _cartRepository.GetAllCartItems(cartId);
        }
        public async Task<int?> GetCartIdByUserId(int userId)
        {
            return await _cartRepository.GetCartIdByUserId(userId);
        }

        public async Task<CartItemDto> AddCartItemInCart(int productId, int cartId, int quantity)
        {
            var cartValidationResult = _cartIdValidator.Validate(cartId);
            if (!cartValidationResult.IsValid)
            {
                throw new ValidationException(cartValidationResult.Errors);
            }

            var product = await _productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            if (quantity <= 0)
            {
                throw new ValidationException("Quantity must be greater than zero.");
            }

            if (quantity > product.StockQuantity)
            {
                throw new ValidationException($"Requested quantity exceeds available stock");
            }

            return await _cartRepository.AddCartItemInCart(productId, cartId, quantity);
        }

        public async Task<bool> RemoveCartItemInsideOfCartByCartItemId(int cartItemId)
        {
            var validationResult = _cartItemIdValidator.Validate(cartItemId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _cartRepository.RemoveCartItemInsideOfCartByCartItemId(cartItemId);
        }

        public async Task<bool> ClearCart(int cartId)
        {
            var validationResult = _cartIdValidator.Validate(cartId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _cartRepository.ClearCart(cartId);
        }

        public async Task<decimal> GetCartTotal(int cartId)
        {
            var validationResult = _cartIdValidator.Validate(cartId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _cartRepository.GetCartTotal(cartId);
        }
        public async Task<decimal> GetShippingPrice(int cartId)
        {
            var validationResult = _cartIdValidator.Validate(cartId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await _cartRepository.GetShippingPrice(cartId);
        }
        public async Task ValidateCartOrThrow(int cartId)
        {
            bool isValid = await _cartRepository.ValidateCart(cartId);

            if (!isValid)
            {
                throw new InvalidOperationException("Cart items stock has changed, the cart has been updated.");
            }
        }
        public async Task<int> GetCartItemCountByUserIdService(int userId)
        {
            var cartId = await _cartRepository.GetCartIdByUserId(userId);
            if (cartId == null)
            {
                throw new InvalidOperationException("Cart not found for the user.");
            }

            return await _cartRepository.GetCartItemsCountByUserId(userId);
        }
    }
}