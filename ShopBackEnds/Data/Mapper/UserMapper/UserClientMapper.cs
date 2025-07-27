using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.Data.Mapper.UserMapper
{
    public class UserClientMapper
    {
        public static UserDtoClient ToClientDto(User user)
        {
            if (user == null) return null;
            return new UserDtoClient
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserAccessType = user.UserAccessType,
                LastModifyDate = user.LastModifyDate,
                CreationDate = user.CreationDate,
                IsDeleted = user.IsDeleted,
                EmailConfirmed = user.EmailConfirmed
                
            };
        }
    }
}
