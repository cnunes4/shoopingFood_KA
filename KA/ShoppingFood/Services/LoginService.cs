using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json;
using ShoopingFood.Interfaces;
using ShoopingFood.Models;
using System.Security.Cryptography;
using System.Text;

namespace ShoopingFood.Services
{
    public class LoginService : ILoginService
    {

        private readonly ILogger<LoginService> _logger;
        private readonly HttpClient _httpClient;

        public LoginService(ILogger<LoginService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("LoginApi");
        }

        public async Task<UserResponse> Login(User user)
        {
            user.Password = HashPassword(user.Username, user.Password);
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
           
            var response = await _httpClient.PostAsync("api/login", content);
            if (response.IsSuccessStatusCode)
            {

                var responseData = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseData);
                return new UserResponse() {
                    Success=true, 
                    Token= jsonResponse?.SelectToken("token")?.ToString()
                };
               
            }

            return new UserResponse()
            {
                Success = false
            };
        }


        private string HashPassword(string username, string password)
        {
            byte[] salt = GenerateSaltFromName(username);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Combine salt and hashed password to store in the database
            return $"{Convert.ToBase64String(salt)}${hashedPassword}";
        }

        private byte[] GenerateSaltFromName(string username, int saltSize = 16)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Name must not be null or empty.");

            using (var sha256 = SHA256.Create())
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(username);
                byte[] hash = sha256.ComputeHash(nameBytes);

                byte[] salt = new byte[saltSize];
                Array.Copy(hash, salt, saltSize);

                return salt;
            }
        }

    }
}

