using ShopBackEnd.Data.Dto;
using ShopBackEnd.HelperClass;
using ShopBackEnd.HelperClass.JWT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public interface IUserRepository
    {
        Task<AuthResponseDto> ConnectAsync(string email, string password);
        Task<bool> AuthenticateAdminAsync(string email, string password);
        Task<UserDtoClient> AuthenticateEmployeeAsync(string email, string password);
        Task<UserDtoClient> GetUserByIdAsync(int id);
        Task<UserDtoClient> GetUserByEmailAsync(string email);
        Task<UserDto> GetFullUserDetailsByEmailAsync(string email); // New method for internal use
        Task<PagedResult<UserDtoClient>> GetAllUsersAsync(int pageNumber, int pageSize);
        Task<PagedResult<UserDtoClient>> GetAllAdminsAsync(int pageNumber, int pageSize);
        Task<PagedResult<UserDtoClient>> GetAllEmployeesAsync(int pageNumber, int pageSize);
        Task<PagedResult<UserDtoClient>> GetAllCustomersAsync(int pageNumber, int pageSize);
        Task<PagedResult<UserDtoClient>> GetUsersByNameAsync(string name, int pageNumber, int pageSize);
        Task<PagedResult<UserDtoClient>> GetUsersByEmailAsync(string email, int pageNumber, int pageSize);
        Task<UserDtoClient> AddUserAsync(UserDtoAdd userDtoAdd);
        Task<UserDtoClient> EditUserAsync(int userId, UserDtoEdit userDtoEdit);
        Task<UserDtoClient> EditUserPasswordAsync(int userId, UserDtoEditPassword userDtoEditPassword);
        Task<bool> DeleteUserAsync(int userId);
        Task<int> CountActiveUsersAsync();
        Task<UserDtoClient> SetUserAsAdminAsync(int userId);
        Task<UserDtoClient> SetUserAsEmployeeAsync(int userId);
        Task<UserDtoClient> ConfirmEmailAsync(string token);
        Task<bool> RequestPasswordResetAsync(string email);
        Task<UserDtoClient> ResetPasswordAsync(string token, string newPassword);
    }
}
