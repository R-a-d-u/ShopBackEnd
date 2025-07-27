using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
namespace ShopBackEnd.Data.Mapper.UserMapper;

public class UserAddMapper
{
     
    public static User ToEntity(UserDtoAdd userDtoAdd)
    {
        if (userDtoAdd == null) return null;

        return new User
        {
            Name = userDtoAdd.Name,
            Email = userDtoAdd.Email,
            Password = userDtoAdd.Password,   
            PhoneNumber = userDtoAdd.PhoneNumber,
            UserAccessType = userDtoAdd.UserAccessType,
            LastModifyDate = userDtoAdd.LastModifyDate,
            IsDeleted = userDtoAdd.IsDeleted,
            EmailConfirmed = userDtoAdd.EmailConfirmed,
        };
    }
}
