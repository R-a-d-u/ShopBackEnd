using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Dto
{
    public class UserDtoAdd
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "@gmail.com";
        public string Password { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public UserAccesType UserAccessType { get; set; }
        public DateTime LastModifyDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public bool EmailConfirmed { get; set; } = false;
    }
}
