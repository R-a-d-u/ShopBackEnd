using ShopBackEnd.Data.Dto;

namespace ShopBackEnd.HelperClass.JWT
{
    public class AuthResponseDto
    {
        public UserDtoClient User { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
