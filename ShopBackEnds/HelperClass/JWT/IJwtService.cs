using System.Security.Claims;

namespace ShopBackEnd.HelperClass.JWT
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal ValidateToken(string token);
    }
}
