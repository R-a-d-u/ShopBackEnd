using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{


    public static string HashPassword(string password)
    {
        //  salt = Guid.NewGuid().ToString();
        //  using var sha256 = SHA256.Create();
        //  var saltedPassword = password + salt;
        //  var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        //  return Convert.ToBase64String(bytes);
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
       // using var sha256 = SHA256.Create();
       // var saltedPassword = password + storedSalt;
       // var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
       // var computedHash = Convert.ToBase64String(bytes);
       // return computedHash == storedHash;

        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }
}


