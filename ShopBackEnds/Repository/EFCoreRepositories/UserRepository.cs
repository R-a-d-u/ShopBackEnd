using Microsoft.EntityFrameworkCore;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
using ShopBackEnd.Repository.Context;
using System.Linq;
using System.Threading.Tasks;
using ShopBackEnd.Data.Mapper.UserMapper;
using ShopBackEnd.Service;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.HelperClass;
using ShopBackEnd.Services;
using System.Security.Cryptography;
using ShopBackEnd.HelperClass.JWT;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtService _jwtService;

        public UserRepository(ShopDbContext context, ILogger<UserService> logger, IEmailService emailService, IJwtService jwtService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
            _jwtService= jwtService; 
        }

        public async Task<AuthResponseDto> ConnectAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            if (user == null)
            {
                throw new InvalidOperationException("The email was not found or is not registered.");
            }

            if (!user.EmailConfirmed)
            {
                if (user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
                {
                    
                    user.EmailConfirmationToken = GenerateToken();
                    user.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(7); 
                    await _context.SaveChangesAsync();

                   
                    await _emailService.SendConfirmationEmailAsync(user.Email, user.EmailConfirmationToken);

                    throw new InvalidOperationException("Check your email.");
                }

                throw new InvalidOperationException("The email was not confirmed.");
            }

            
            if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                throw new UnauthorizedAccessException("Password is not correct.");
            }
            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                User = UserClientMapper.ToClientDto(user),
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<Boolean> AuthenticateAdminAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted && u.UserAccessType == UserAccesType.Admin);

            if (user == null)
            {
                return false;
            }
            if (user.EmailConfirmed == false)
            {
                return false;
            }

            if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                return false;
            }

            return true;
        }

        public async Task<UserDtoClient> AuthenticateEmployeeAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted && u.UserAccessType == UserAccesType.Employee);

            if (user == null)
            {
                throw new InvalidOperationException("Employee email not found or user is not an employee.");
            }
            if (user.EmailConfirmed == false)
            {
                throw new InvalidOperationException("The email was not confirmed.");
            }

            if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                throw new UnauthorizedAccessException("Password is incorrect.");
            }

            return UserClientMapper.ToClientDto(user);
        }

        public async Task<UserDtoClient> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new InvalidOperationException("User id not found.");
            }

            return UserClientMapper.ToClientDto(user);
        }

        public async Task<UserDtoClient> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            if (user == null)
            {
                throw new InvalidOperationException("User with this email not found.");
            }

            return UserClientMapper.ToClientDto(user);
        }

        public async Task<PagedResult<UserDtoClient>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            var query = _context.Users.AsNoTracking();
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<UserDtoClient>(
                items.Select(UserClientMapper.ToClientDto),
                totalItems,
                pageNumber,
                pageSize
            );
        }

        public async Task<PagedResult<UserDtoClient>> GetAllAdminsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Users
                .Where(u => u.UserAccessType == UserAccesType.Admin && u.EmailConfirmed == true)
                .AsNoTracking();

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<UserDtoClient>(
                items.Select(UserClientMapper.ToClientDto),
                totalItems,
                pageNumber,
                pageSize
            );
        }

        public async Task<PagedResult<UserDtoClient>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            var query = _context.Users
               .Where(u => u.UserAccessType == UserAccesType.Employee && u.EmailConfirmed == true)
               .AsNoTracking();

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<UserDtoClient>(
                items.Select(UserClientMapper.ToClientDto),
                totalItems,
                pageNumber,
                pageSize
            );
        }

        public async Task<PagedResult<UserDtoClient>> GetAllCustomersAsync(int pageNumber, int pageSize)
        {
            var query = _context.Users
               .Where(u => u.UserAccessType == UserAccesType.Customer && u.EmailConfirmed == true)
               .AsNoTracking();

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<UserDtoClient>(
                items.Select(UserClientMapper.ToClientDto),
                totalItems,
                pageNumber,
                pageSize
            );
        }

        public async Task<PagedResult<UserDtoClient>> GetUsersByNameAsync(string name, int pageNumber, int pageSize)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.Name.Contains(name) && u.EmailConfirmed == true);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<UserDtoClient>(
                items.Select(UserClientMapper.ToClientDto),
                totalItems,
                pageNumber,
                pageSize
            );
        }

        public async Task<PagedResult<UserDtoClient>> GetUsersByEmailAsync(string email, int pageNumber, int pageSize)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.Email.Contains(email) && u.EmailConfirmed == true);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<UserDtoClient>(
                items.Select(UserClientMapper.ToClientDto),
                totalItems,
                pageNumber,
                pageSize
            );
        }

        public async Task<UserDtoClient> AddUserAsync(UserDtoAdd userDtoAdd)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userDtoAdd.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("The email address is already registered.");
            }

            // Hash the password
            string passwordHash = PasswordHasher.HashPassword(userDtoAdd.Password);
            string confirmationToken = GenerateToken();

            var user = new User
            {
                Name = userDtoAdd.Name,
                Email = userDtoAdd.Email,
                Password = passwordHash,
                PhoneNumber = userDtoAdd.PhoneNumber,
                UserAccessType = userDtoAdd.UserAccessType,
                LastModifyDate = userDtoAdd.LastModifyDate,
                IsDeleted = false,
                EmailConfirmed = false,
                EmailConfirmationToken = confirmationToken,
                EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(7),
                CreationDate= DateTime.UtcNow,
            };

            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("SaveChanges succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveChanges failed: {ex.ToString()}");
                throw; 
            }

            var cart = new Cart
            {
                UserId = user.Id,
                Items = new List<CartItem>()
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return UserClientMapper.ToClientDto(user);
        }

        public async Task<UserDtoClient> EditUserAsync(int userId, UserDtoEdit userDtoEdit)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User id not found.");
            }

            var updatedUser = UserEditMapper.ToEntity(userDtoEdit, existingUser);

            _context.Users.Update(updatedUser);
            await _context.SaveChangesAsync();

            return UserClientMapper.ToClientDto(updatedUser);
        }

        public async Task<UserDtoClient> EditUserPasswordAsync(int userId, UserDtoEditPassword userDtoEditPassword)
        {
            var existingUser = await _context.Users.FindAsync(userId);

            if (existingUser == null)
            {
                throw new InvalidOperationException("User ID not found.");
            }

            string passwordHash = PasswordHasher.HashPassword(userDtoEditPassword.Password);

            existingUser.Password = passwordHash;
            existingUser.LastModifyDate = userDtoEditPassword.LastModifyDate;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return UserClientMapper.ToClientDto(existingUser);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User id not found.");
            }
            user.IsDeleted = true;
            user.LastModifyDate = DateTime.UtcNow;
            user.Email = $"{user.Email}_deleted_{Guid.NewGuid()}";
            user.Name = "Empty";
            user.PhoneNumber = "Empty";
            user.PasswordResetToken = "";

            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                cart.Items.Clear();
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CountActiveUsersAsync()
        {
            return await _context.Users
               .CountAsync(u => !u.IsDeleted && u.UserAccessType == UserAccesType.Customer && u.EmailConfirmed);
        }

        public async Task<UserDtoClient> SetUserAsAdminAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted)
            {
                throw new InvalidOperationException("User not found or has been deleted.");
            }

            user.UserAccessType = UserAccesType.Admin;
            user.LastModifyDate = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return UserClientMapper.ToClientDto(user);
        }

        public async Task<UserDtoClient> SetUserAsEmployeeAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted)
            {
                throw new InvalidOperationException("User not found or has been deleted.");
            }

            user.UserAccessType = UserAccesType.Employee;
            user.LastModifyDate = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return UserClientMapper.ToClientDto(user);
        }

        public async Task<UserDtoClient> ConfirmEmailAsync(string token)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token && !u.IsDeleted);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid or expired confirmation token.");
            }

            if (user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Confirmation token has expired.");
            }

            user.EmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiry = null;
            user.LastModifyDate = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return UserClientMapper.ToClientDto(user);
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            if (user == null)
            {
                return false;
            }
            if (user.PasswordResetTokenExpiry.HasValue && user.PasswordResetTokenExpiry > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Check your email for the reset token.");
            }

            string resetToken = GenerateToken();

            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);
            user.LastModifyDate = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDtoClient> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.PasswordResetToken == token && !u.IsDeleted);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid reset token.");
            }

            if (user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Reset token has expired.");
            }

            string passwordHash = PasswordHasher.HashPassword(newPassword);
            user.Password = passwordHash;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            user.LastModifyDate = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return UserClientMapper.ToClientDto(user);
        }
        public async Task<UserDto> GetFullUserDetailsByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            if (user == null)
            {
                throw new InvalidOperationException("User with this email not found.");
            }

            return UserMapper.ToDto(user); 
        }

        private string GenerateToken(int length = 16)
        {
            var tokenBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }

            return Convert.ToBase64String(tokenBytes)
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "")
                .Substring(0, length); 
        }
    }
}