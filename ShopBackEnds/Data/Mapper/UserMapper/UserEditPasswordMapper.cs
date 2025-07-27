using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
namespace ShopBackEnd.Data.Mapper.UserMapper;

public class UserEditPasswordMapper
{
     
    public static User ToEntity(UserDtoEditPassword userDtoEditPassword, User existingUser)
    {
        if (userDtoEditPassword == null || existingUser == null) return null;

        existingUser.Password = userDtoEditPassword.Password;  
        existingUser.LastModifyDate = DateTime.Now;  

        return existingUser;
    }
}
