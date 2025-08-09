using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Validators.ResponseValidator;
using ShopBackEnd.Service;
using ShopBackEnd.HelperClass;
using ShopBackEnd.HelperClass.JWT;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Connect", Name = "ConnectUser")]
    public async Task<ActionResult<ResponseValidator<AuthResponseDto>>> Connect([FromBody] UserDtoConnect userDtoConnect)
    {
        try
        {
            var user = await _userService.ConnectAsync(userDtoConnect);
            return Ok(ResponseValidator<AuthResponseDto>.Success(user));
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(ResponseValidator<AuthResponseDto>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<AuthResponseDto>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPost("AuthenticateAdmin", Name = "AuthenticateAdmin")]
    public async Task<ActionResult<ResponseValidator<Boolean>>> AuthenticateAdmin([FromBody] UserDtoConnect userDtoConnect)
    {
        try
        {
            var user = await _userService.AuthenticateAdminAsync(userDtoConnect);
            return Ok(ResponseValidator<Boolean>.Success(user));
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(ResponseValidator<Boolean>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<Boolean>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPost("AuthenticateEmployee", Name = "AuthenticateEmployee")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> AuthenticateEmployee([FromBody] UserDtoConnect userDtoConnect)
    {
        try
        {
            var user = await _userService.AuthenticateEmployeeAsync(userDtoConnect);
            return Ok(ResponseValidator<UserDtoClient>.Success(user));
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(ResponseValidator<UserDtoClient>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetAll", Name = "GetAllUsers")]
    public async Task<ActionResult<ResponseValidator<PagedResult<UserDtoClient>>>> GetAllUsers(
        [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedResult = await _userService.GetAllUsersAsync(page, 20);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<UserDtoClient>>.Failure("The user list is empty."));
            }
            return Ok(ResponseValidator<PagedResult<UserDtoClient>>.Success(pagedResult));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<PagedResult<UserDtoClient>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<UserDtoClient>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetAllAdmins", Name = "GetAllAdmins")]
    public async Task<ActionResult<ResponseValidator<PagedResult<UserDtoClient>>>> GetAllAdmins(
        [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedResult = await _userService.GetAllAdminsAsync(page, 20);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<UserDtoClient>>.Failure("The user list is empty."));
            }
            return Ok(ResponseValidator<PagedResult<UserDtoClient>>.Success(pagedResult));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<PagedResult<UserDtoClient>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<UserDtoClient>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetAllEmployees", Name = "GetAllEmployees")]
    public async Task<ActionResult<ResponseValidator<PagedResult<UserDtoClient>>>> GetAllEmployees(
       [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedResult = await _userService.GetAllEmployeesAsync(page, 20);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<UserDtoClient>>.Failure("The user list is empty."));
            }
            return Ok(ResponseValidator<PagedResult<UserDtoClient>>.Success(pagedResult));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<PagedResult<UserDtoClient>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<UserDtoClient>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetAllCustomers", Name = "GetAllCustomers")]
    public async Task<ActionResult<ResponseValidator<PagedResult<UserDtoClient>>>> GetAllCustomers(
        [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedResult = await _userService.GetAllCustomersAsync(page, 20);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<UserDtoClient>>.Failure("The user list is empty."));
            }
            return Ok(ResponseValidator<PagedResult<UserDtoClient>>.Success(pagedResult));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<PagedResult<UserDtoClient>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<UserDtoClient>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetByName", Name = "GetUsersByName")]
    public async Task<ActionResult<ResponseValidator<PagedResult<UserDtoClient>>>> GetUsersByName(
    [FromQuery] string name,
    [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedResult = await _userService.GetUsersByNameAsync(name, page, 20);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<UserDtoClient>>.Failure("No users found with this name."));
            }
            return Ok(ResponseValidator<PagedResult<UserDtoClient>>.Success(pagedResult));
        }
        catch (ArgumentException e)
        {
            return BadRequest(ResponseValidator<PagedResult<UserDtoClient>>.Failure(e.Message));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<PagedResult<UserDtoClient>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<UserDtoClient>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("GetByEmail", Name = "GetUsersByEmail")]
    public async Task<ActionResult<ResponseValidator<PagedResult<UserDtoClient>>>> GetUsersByEmail(
        [FromQuery] string email,
        [FromQuery(Name = "page")] int page = 1)
    {
        try
        {
            var pagedResult = await _userService.GetUsersByEmailAsync(email, page, 20);
            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return NotFound(ResponseValidator<PagedResult<UserDtoClient>>.Failure("No users found with this email."));
            }
            return Ok(ResponseValidator<PagedResult<UserDtoClient>>.Success(pagedResult));
        }
        catch (ArgumentException e)
        {
            return BadRequest(ResponseValidator<PagedResult<UserDtoClient>>.Failure(e.Message));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<PagedResult<UserDtoClient>>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<PagedResult<UserDtoClient>>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("Get/{id}", Name = "GetUserById")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(ResponseValidator<UserDtoClient>.Success(user));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<UserDtoClient>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<UserDtoClient>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpGet("CountActiveUsers", Name = "CountActiveUsers")]
    public async Task<ActionResult<ResponseValidator<int>>> CountActiveUsers()
    {
        try
        {
            var activeUserCount = await _userService.CountActiveUsersAsync();
            return Ok(ResponseValidator<int>.Success(activeUserCount));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<int>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPost("Add", Name = "AddUser")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> AddUser([FromBody] UserDtoAdd userDtoAdd)
    {
        try
        {
            var addedUser = await _userService.AddUserAsync(userDtoAdd);
            return Ok(ResponseValidator<UserDtoClient>.Success(addedUser));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<UserDtoClient>.Failure("Validation error: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("Edit/{userId}", Name = "EditUser")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> EditUser(int userId, [FromBody] UserDtoEdit userDtoEdit)
    {
        try
        {
            var updatedUser = await _userService.EditUserAsync(userId, userDtoEdit);
            return Ok(ResponseValidator<UserDtoClient>.Success(updatedUser));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<UserDtoClient>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<UserDtoClient>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("EditPassword/{userId}", Name = "EditUserPassword")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> EditUserPassword(int userId, [FromBody] UserDtoEditPassword userDtoEditPassword)
    {
        try
        {
            var updatedUser = await _userService.EditUserPasswordAsync(userId, userDtoEditPassword);
            return Ok(ResponseValidator<UserDtoClient>.Success(updatedUser));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<UserDtoClient>.Failure("Validation error: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPut("SetUserAsAdmin/{userId}", Name = "SetAsAdmin")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> SetUserAsAdmin(int userId)
    {
        try
        {
            var updatedUser = await _userService.SetUserAsAdminAsync(userId);
            return Ok(ResponseValidator<UserDtoClient>.Success(updatedUser));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ResponseValidator<UserDtoClient>.Failure(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"An error occurred: {ex.Message}"));
        }
    }

    [HttpPut("SetUserAsEmployee/{userId}", Name = "SetAsEmployee")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> SetUserAsEmployee(int userId)
    {
        try
        {
            var updatedUser = await _userService.SetUserAsEmployeeAsync(userId);
            return Ok(ResponseValidator<UserDtoClient>.Success(updatedUser));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ResponseValidator<UserDtoClient>.Failure(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"An error occurred: {ex.Message}"));
        }
    }

    [HttpDelete("Delete/{userId}", Name = "DeleteUser")]
    public async Task<ActionResult<ResponseValidator<bool>>> DeleteUser(int userId)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
            {
                return BadRequest(ResponseValidator<bool>.Failure($"The user with ID {userId} could not be deleted."));
            }
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure("A validation error occurred: " + e.Errors.FirstOrDefault()?.ErrorMessage));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"An error occurred: {e.Message}"));
        }
    }

    [HttpPost("ConfirmEmail", Name = "ConfirmEmail")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> ConfirmEmail([FromBody] EmailConfirmationRequest request)
    {
        try
        {
            var user = await _userService.ConfirmEmailAsync(request.Token);
            return Ok(ResponseValidator<UserDtoClient>.Success(user));
        }
        catch (ArgumentException e)
        {
            return BadRequest(ResponseValidator<UserDtoClient>.Failure(e.Message));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<UserDtoClient>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"{e.Message}"));
        }
    }

    [HttpPost("RequestPasswordReset", Name = "RequestPasswordReset")]
    public async Task<ActionResult<ResponseValidator<bool>>> RequestPasswordReset([FromBody] PasswordResetRequest request)
    {
        try
        {
            var result = await _userService.RequestPasswordResetAsync(request.Email);
            return Ok(ResponseValidator<bool>.Success(result));
        }
        catch (ArgumentException e)
        {
            return BadRequest(ResponseValidator<bool>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<bool>.Failure($"{e.Message}"));
        }
    }

    [HttpPost("ResetPassword", Name = "ResetPassword")]
    public async Task<ActionResult<ResponseValidator<UserDtoClient>>> ResetPassword([FromBody] PasswordResetConfirmation request)
    {
        try
        {
            var user = await _userService.ResetPasswordAsync(request.Token, request.NewPassword);
            return Ok(ResponseValidator<UserDtoClient>.Success(user));
        }
        catch (ArgumentException e)
        {
            return BadRequest(ResponseValidator<UserDtoClient>.Failure(e.Message));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ResponseValidator<UserDtoClient>.Failure(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ResponseValidator<UserDtoClient>.Failure($"{e.Message}"));
        }
    }

    // Request models needed for the new endpoints
    public class EmailConfirmationRequest
    {
        public string Token { get; set; }
    }

    public class PasswordResetRequest
    {
        public string Email { get; set; }
    }

    public class PasswordResetConfirmation
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}