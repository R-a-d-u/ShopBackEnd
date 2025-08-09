using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Services;
using ShopBackEnd.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopBackEnd.Validators.ResponseValidator;

[ApiController]
[Route("[controller]")]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpGet("Get/{cartItemId}", Name = "GetCartItem")]
    public async Task<ActionResult<ResponseValidator<CartItemDto>>> GetCartItem(int cartItemId)
    {
        try
        {
            var cartItem = await _cartItemService.GetCartItemById(cartItemId);
            return Ok(ResponseValidator<CartItemDto>.Success(cartItem));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<CartItemDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<CartItemDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<CartItemDto>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("UpdateQuantity", Name = "UpdateCartItemQuantity")]
    public async Task<ActionResult<ResponseValidator<CartItemDto>>> UpdateCartItemQuantity([FromBody] CartItemUpdateRequest request)
    {
        try
        {

            var result = await _cartItemService.UpdateCartItemQuantity(request.CartItemId, request.Quantity);
            return Ok(ResponseValidator<CartItemDto>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<CartItemDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
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
}

public class CartItemUpdateRequest
{
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
}
