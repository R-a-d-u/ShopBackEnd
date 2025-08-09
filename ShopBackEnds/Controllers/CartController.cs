using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.HelperClass;
using ShopBackEnd.Services;
using ShopBackEnd.Validators.ResponseValidator;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("GetCartId/{userId}", Name = "GetCartIdByUserId")]
    public async Task<ActionResult<ResponseValidator<int?>>> GetCartIdByUserId(int userId)
    {
        try
        {
            var cartId = await _cartService.GetCartIdByUserId(userId);
            if (cartId == null)
            {
                return NotFound(ResponseValidator<int?>.Failure("No cart found for this user."));
            }
            return Ok(ResponseValidator<int?>.Success(cartId));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<int?>.Failure($"An error occurred: {e.Message}"));
        }

    }

    [HttpGet("GetAllItems/{cartId}", Name = "GetAllCartItems")]
    public async Task<ActionResult<ResponseValidator<List<CartItemDto>>>> GetAllCartItems(int cartId)
    {
        try
        {
            var cartItems = await _cartService.GetAllCartItems(cartId);
            if (cartItems == null || !cartItems.Any())
            {
                return NotFound(ResponseValidator<PagedResult<UserDto>>.Failure("Cart is empty."));
            }
            return Ok(ResponseValidator<List<CartItemDto>>.Success(cartItems));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<List<CartItemDto>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(ResponseValidator<List<CartItemDto>>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<List<CartItemDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPost("AddItem", Name = "AddCartItem")]
    public async Task<ActionResult<ResponseValidator<CartItemDto>>> AddCartItem([FromBody] CartItemAddRequest request)
    {
        try
        {
            var cartItem = await _cartService.AddCartItemInCart(
                request.ProductId,
                request.CartId,
                request.Quantity);

            return Ok(ResponseValidator<CartItemDto>.Success(cartItem));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<CartItemDto>.Failure("A validation error occurred: " + e.Message));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<CartItemDto>.Failure(e.Message));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(ResponseValidator<CartItemDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<CartItemDto>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpDelete("RemoveItem/{cartItemId}", Name = "RemoveCartItem")]
    public async Task<ActionResult<ResponseValidator<bool>>> RemoveCartItem(int cartItemId)
    {
        try
        {
            var result = await _cartService.RemoveCartItemInsideOfCartByCartItemId(cartItemId);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpDelete("Clear/{cartId}", Name = "ClearCart")]
    public async Task<ActionResult<ResponseValidator<bool>>> ClearCart(int cartId)
    {
        try
        {
            var result = await _cartService.ClearCart(cartId);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetTotal/{cartId}", Name = "GetCartTotal")]
    public async Task<ActionResult<ResponseValidator<decimal>>> GetCartTotal(int cartId)
    {
        try
        {
            var total = await _cartService.GetCartTotal(cartId);
            return Ok(ResponseValidator<decimal>.Success(total));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<decimal>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(ResponseValidator<decimal>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<decimal>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("GetShippingPrice/{cartId}", Name = "GetShippingPrice")]
    public async Task<ActionResult<ResponseValidator<decimal>>> GetShippingPrice(int cartId)
    {
        try
        {
            var shippingPrice = await _cartService.GetShippingPrice(cartId);
            return Ok(ResponseValidator<decimal>.Success(shippingPrice));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<decimal>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(ResponseValidator<decimal>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<decimal>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("GetCartItemCount/{userId}", Name = "GetCartItemCountByUserId")]
    public async Task<ActionResult<ResponseValidator<int>>> GetCartItemCountByUserId(int userId)
    {
        try
        {
            var itemCount = await _cartService.GetCartItemCountByUserIdService(userId);
            return Ok(ResponseValidator<int>.Success(itemCount));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<int>.Failure($"An error occurred: {e.Message}"));
        }
    }
}



public class CartItemAddRequest
{
    public int ProductId { get; set; }
    public int CartId { get; set; }
    public int Quantity { get; set; }
}