using Microsoft.Data.SqlClient;
using System.Data;
using WeatherApp.Models;
using WeatherApp.Helpers;

namespace WeatherApp.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection not found in configuration");
        }

        public async Task<User?> AuthenticateAsync(string username, string enteredPassword)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null)
                return null;

            bool isValid = PasswordHasher.VerifyPassword(enteredPassword, user.PasswordHash, user.Salt);

            return isValid ? user : null;
        }

        internal async Task ValidateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
{
    using SqlConnection conn = new SqlConnection(_connectionString);
    using SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @Username", conn);
    cmd.Parameters.AddWithValue("@Username", username);

    await conn.OpenAsync();
    using var reader = await cmd.ExecuteReaderAsync();
    if (await reader.ReadAsync())
    {
        var foundUsername = reader.GetString(1);

        return new User
        {
            Id = reader.GetInt32(0),
            Username = foundUsername,
            PasswordHash = reader.GetString(2),
            Salt = reader.GetString(3)
        };
    }
    return null;
}


    }
}
