using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Validators.ResponseValidator;
using ShopBackEnd.Services;
using ShopBackEnd.Data.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShopBackEnd.HelperClass;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("CreateFromCart", Name = "CreateOrderFromCart")]
    public async Task<ActionResult<ResponseValidator<OrderDtoAdd>>> CreateOrderFromCart([FromBody] OrderCreationRequest request)
    {
        try
        {
            var orderDto = await _orderService.CreateOrderFromCart(request.CartId, request.UserAddress, request.PaymentMethod);
            return Ok(ResponseValidator<OrderDtoAdd>.Success(orderDto));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<OrderDtoAdd>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<OrderDtoAdd>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<OrderDtoAdd>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("Get/{orderId}", Name = "GetOrderById")]
    public async Task<ActionResult<ResponseValidator<OrderDto>>> GetOrderById([FromRoute] Guid orderId)
    {
        try
        {
            var order = await _orderService.GetOrderById(orderId);
            return Ok(ResponseValidator<OrderDto>.Success(order));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<OrderDto>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<OrderDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<OrderDto>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetAllByUser/{userId}", Name = "GetAllOrdersByUserId")]
    public async Task<ActionResult<ResponseValidator<PagedResult<OrderDto>>>> GetAllOrdersByUserId(
        [FromRoute] int userId,
        [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedOrders = await _orderService.GetAllOrdersByUserId(userId, page, 10);

            if (pagedOrders.Items == null || !pagedOrders.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<OrderDto>>.Failure("No orders found for this user."));
            }

            return Ok(ResponseValidator<PagedResult<OrderDto>>.Success(pagedOrders));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<PagedResult<OrderDto>>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<OrderDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("GetAllOrdersByOrderStatus/{orderStatusId}", Name = "GetAllOrdersByOrderStatus")]
    public async Task<ActionResult<ResponseValidator<PagedResult<OrderDto>>>> GetAllOrders(
       [FromRoute] OrderStatus orderStatusId,
       [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedOrders = await _orderService.GetAllOrdersByOrderStatus(orderStatusId,page, 10);

            if (pagedOrders.Items == null || !pagedOrders.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<OrderDto>>.Failure("No orders found for this user."));
            }

            return Ok(ResponseValidator<PagedResult<OrderDto>>.Success(pagedOrders));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<PagedResult<OrderDto>>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<OrderDto>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("UpdateStatusToProcessing/{orderId}", Name = "UpdateOrderStatusToProcessing")]
    public async Task<ActionResult<ResponseValidator<bool>>> UpdateOrderStatusToProcessing(Guid orderId)
    {
        try
        {
            await _orderService.UpdateOrderStatusToProcessing(orderId);
            return Ok(ResponseValidator<bool>.Success(true));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("UpdateStatusToDelivered/{orderId}", Name = "UpdateOrderStatusToDelivered")]
    public async Task<ActionResult<ResponseValidator<bool>>> UpdateOrderStatusToDelivered(Guid orderId)
    {
        try
        {
            await _orderService.UpdateOrderStatusToDelivered(orderId);
            return Ok(ResponseValidator<bool>.Success(true));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpPut("UpdateStatusToShipping/{orderId}", Name = "UpdateStatusToShipping")]
    public async Task<ActionResult<ResponseValidator<bool>>> UpdateStatusToShipping(Guid orderId)
    {
        try
        {
            await _orderService.UpdateOrderStatusToShipping(orderId);
            return Ok(ResponseValidator<bool>.Success(true));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("UpdateStatusToReturned/{orderId}", Name = "UpdateOrderStatusToReturned")]
    public async Task<ActionResult<ResponseValidator<bool>>> UpdateOrderStatusToReturned(Guid orderId)
    {
        try
        {
            await _orderService.UpdateOrderStatusToReturned(orderId);
            return Ok(ResponseValidator<bool>.Success(true));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("UpdateStatusToCanceled/{orderId}", Name = "UpdateOrderStatusToCanceled")]
    public async Task<ActionResult<ResponseValidator<bool>>> UpdateOrderStatusToCanceled(Guid orderId)
    {
        try
        {
            await _orderService.UpdateOrderStatusToCanceled(orderId);
            return Ok(ResponseValidator<bool>.Success(true));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }
    [HttpGet("CountCreatedAndPending", Name = "CountCreatedAndPendingOrders")]
    public async Task<ActionResult<ResponseValidator<int>>> CountCreatedAndPendingOrders()
    {
        try
        {
            var count = await _orderService.CountCreatedAndPendingOrders();
            return Ok(ResponseValidator<int>.Success(count));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<int>.Failure($"An error occurred: {e.Message}"));
        }
    }
}

public class OrderCreationRequest
{
    public int CartId { get; set; }
    public string UserAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
