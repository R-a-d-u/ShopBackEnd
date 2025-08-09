using FluentValidation;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.HelperClass;
using ShopBackEnd.HelperClass.JWT;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Services;
using ShopBackEnd.Validation.UserValidation;

namespace ShopBackEnd.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly UserIdValidation _userIdValidator;
        private readonly UserAddValidation _addUserValidator;
        private readonly UserEditValidation _editUserValidator;
        private readonly UserEditPasswordValidation _editUserPasswordValidator;

        public UserService(
            IUserRepository userRepository,
            IEmailService emailService,
            UserIdValidation userIdValidator,
            UserAddValidation addUserValidator,
            UserEditValidation editUserValidator,
            UserEditPasswordValidation editUserPasswordValidator)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _userIdValidator = userIdValidator;
            _addUserValidator = addUserValidator;
            _editUserValidator = editUserValidator;
            _editUserPasswordValidator = editUserPasswordValidator;
        }

        public async Task<AuthResponseDto> ConnectAsync(UserDtoConnect userDtoConnect)
        {
            if (string.IsNullOrWhiteSpace(userDtoConnect.Email) || string.IsNullOrWhiteSpace(userDtoConnect.Password))
            {
                throw new ArgumentException("Email and password cannot be empty.");
            }

            return await _userRepository.ConnectAsync(userDtoConnect.Email, userDtoConnect.Password);
        }

        public async Task<Boolean> AuthenticateAdminAsync(UserDtoConnect userDtoConnect)
        {
            if (string.IsNullOrWhiteSpace(userDtoConnect.Email) || string.IsNullOrWhiteSpace(userDtoConnect.Password))
            {
                throw new ArgumentException("Email and password cannot be empty.");
            }

            return await _userRepository.AuthenticateAdminAsync(userDtoConnect.Email, userDtoConnect.Password);
        }

        public async Task<UserDtoClient> AuthenticateEmployeeAsync(UserDtoConnect userDtoConnect)
        {
            if (string.IsNullOrWhiteSpace(userDtoConnect.Email) || string.IsNullOrWhiteSpace(userDtoConnect.Password))
            {
                throw new ArgumentException("Email and password cannot be empty.");
            }

           
            return await _userRepository.AuthenticateEmployeeAsync(userDtoConnect.Email, userDtoConnect.Password);
        }

        public async Task<UserDtoClient> GetUserByIdAsync(int id)
        {
            var validationResult = _userIdValidator.Validate(id);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return user;
        }

        public async Task<PagedResult<UserDtoClient>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            return await _userRepository.GetAllUsersAsync(pageNumber, pageSize);
        }

        public async Task<PagedResult<UserDtoClient>> GetAllAdminsAsync(int pageNumber, int pageSize)
        {
            return await _userRepository.GetAllAdminsAsync(pageNumber, pageSize);
        }

        public async Task<PagedResult<UserDtoClient>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            return await _userRepository.GetAllEmployeesAsync(pageNumber, pageSize);
        }

        public async Task<PagedResult<UserDtoClient>> GetAllCustomersAsync(int pageNumber, int pageSize)
        {
            return await _userRepository.GetAllCustomersAsync(pageNumber, pageSize);
        }

        public async Task<PagedResult<UserDtoClient>> GetUsersByNameAsync(string name, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Search name cannot be empty.");
            }

            return await _userRepository.GetUsersByNameAsync(name, pageNumber, pageSize);
        }

        public async Task<PagedResult<UserDtoClient>> GetUsersByEmailAsync(string email, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Search email cannot be empty.");
            }

            return await _userRepository.GetUsersByEmailAsync(email, pageNumber, pageSize);
        }

        public async Task<UserDtoClient> AddUserAsync(UserDtoAdd userDtoAdd)
        {
            var validationResult = _addUserValidator.Validate(userDtoAdd);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.AddUserAsync(userDtoAdd);

            try
            {
                var fullUserDetails = await _userRepository.GetFullUserDetailsByEmailAsync(user.Email);

              
                var emailSent = await _emailService.SendConfirmationEmailAsync(fullUserDetails.Email, fullUserDetails.EmailConfirmationToken);
            }
            catch (Exception ex)
            {
                
            }

            return user;
        }

        public async Task<UserDtoClient> EditUserAsync(int userId, UserDtoEdit userDtoEdit)
        {
            var idValidationResult = _userIdValidator.Validate(userId);
            if (!idValidationResult.IsValid)
            {
                throw new ValidationException(idValidationResult.Errors);
            }

            var editValidationResult = _editUserValidator.Validate(userDtoEdit);
            if (!editValidationResult.IsValid)
            {
                throw new ValidationException(editValidationResult.Errors);
            }

            var user = await _userRepository.EditUserAsync(userId, userDtoEdit);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            return user;
        }

        public async Task<UserDtoClient> EditUserPasswordAsync(int userId, UserDtoEditPassword userDtoEditPassword)
        {
            var idValidationResult = _userIdValidator.Validate(userId);
            if (!idValidationResult.IsValid)
            {
                throw new ValidationException(idValidationResult.Errors);
            }

            var passwordValidationResult = _editUserPasswordValidator.Validate(userDtoEditPassword);
            if (!passwordValidationResult.IsValid)
            {
                throw new ValidationException(passwordValidationResult.Errors);
            }

            var user = await _userRepository.EditUserPasswordAsync(userId, userDtoEditPassword);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var validationResult = _userIdValidator.Validate(userId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<int> CountActiveUsersAsync()
        {
            return await _userRepository.CountActiveUsersAsync();
        }

        public async Task<UserDtoClient> SetUserAsAdminAsync(int userId)
        {
            var idValidationResult = _userIdValidator.Validate(userId);
            if (!idValidationResult.IsValid)
            {
                throw new ValidationException(idValidationResult.Errors);
            }
            return await _userRepository.SetUserAsAdminAsync(userId);
        }

        public async Task<UserDtoClient> SetUserAsEmployeeAsync(int userId)
        {
            var idValidationResult = _userIdValidator.Validate(userId);
            if (!idValidationResult.IsValid)
            {
                throw new ValidationException(idValidationResult.Errors);
            }
            return await _userRepository.SetUserAsEmployeeAsync(userId);
        }

        public async Task<UserDtoClient> ConfirmEmailAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be empty.");
            }

            return await _userRepository.ConfirmEmailAsync(token);
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }

            var result = await _userRepository.RequestPasswordResetAsync(email);

            if (result)
            {
              
                var fullUserDetails = await _userRepository.GetFullUserDetailsByEmailAsync(email);
                await _emailService.SendPasswordResetEmailAsync(email, fullUserDetails.PasswordResetToken);
            }

            return result;
        }

        public async Task<UserDtoClient> ResetPasswordAsync(string token, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ArgumentException("Token or the new password cannot be empty.");
            }

            return await _userRepository.ResetPasswordAsync(token, newPassword);
        }
    }
}