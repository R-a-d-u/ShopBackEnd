using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class UserDtoClient
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public UserAccesType UserAccessType { get; set; }
        public DateTime LastModifyDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool EmailConfirmed { get; set; }
        // No tokens included
    }
}
