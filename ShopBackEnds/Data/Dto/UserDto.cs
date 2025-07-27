using ShopBackEnd.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShopBackEnd.Data.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "@gmail.com";
        public string PhoneNumber { get; set; } = "";
        public UserAccesType UserAccessType { get; set; }
        public DateTime LastModifyDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpiry { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
    }
}
