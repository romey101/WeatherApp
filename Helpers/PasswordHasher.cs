using System.Security.Cryptography;
using System.Text;

namespace WeatherApp.Helpers
{
    public static class PasswordHasher
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string salt)
        {
            using var hmac = new HMACSHA256();
            var saltBytes = hmac.Key;
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            passwordHash = Convert.ToBase64String(hashBytes);
            salt = Convert.ToBase64String(saltBytes);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
{
    var saltBytes = Convert.FromBase64String(storedSalt);
    using var hmac = new HMACSHA256(saltBytes);
    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

    var computedHashBase64 = Convert.ToBase64String(computedHash);


    return computedHashBase64 == storedHash;
}
    }
}
