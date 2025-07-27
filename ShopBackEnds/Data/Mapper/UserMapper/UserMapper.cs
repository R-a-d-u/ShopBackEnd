using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
namespace ShopBackEnd.Data.Mapper.UserMapper;

public class UserMapper
{
     
    public static UserDto ToDto(User user)
    {
        if (user == null) return null;
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserAccessType = user.UserAccessType,
            LastModifyDate = user.LastModifyDate,
            CreationDate = user.CreationDate,
            IsDeleted = user.IsDeleted,
            EmailConfirmed = user.EmailConfirmed,
            EmailConfirmationToken = user.EmailConfirmationToken,
            EmailConfirmationTokenExpiry = user.EmailConfirmationTokenExpiry,
            PasswordResetToken = user.PasswordResetToken,
            PasswordResetTokenExpiry = user.PasswordResetTokenExpiry
        };
    }

    
    public static User ToEntity(UserDto userDto)
    {
        if (userDto == null) return null;
        return new User
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            UserAccessType = userDto.UserAccessType,
            LastModifyDate = userDto.LastModifyDate,
            CreationDate = userDto.CreationDate,
            IsDeleted = userDto.IsDeleted,
            EmailConfirmed = userDto.EmailConfirmed,
            EmailConfirmationToken = userDto.EmailConfirmationToken,
            EmailConfirmationTokenExpiry = userDto.EmailConfirmationTokenExpiry,
            PasswordResetToken = userDto.PasswordResetToken,
            PasswordResetTokenExpiry = userDto.PasswordResetTokenExpiry
        };
    }
}