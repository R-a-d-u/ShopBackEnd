using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Entity;
namespace ShopBackEnd.Data.Mapper.UserMapper;

public class UserEditMapper
{
    
    public static User ToEntity(UserDtoEdit userDtoEdit, User existingUser)
    {
        if (userDtoEdit == null || existingUser == null) return null;

        existingUser.Name = userDtoEdit.Name;
        existingUser.PhoneNumber = userDtoEdit.PhoneNumber;
        existingUser.LastModifyDate = userDtoEdit.LastModifyDate;

        return existingUser;
    }
}
