using KA.Application.Interfaces;
using KA.Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace KA.Application.Services
{
    public class AuthenticatedService : IAuthenticatedService
    {
        /// <summary>
        /// Convert a pasword in a hash
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>password encripted</returns>
        public string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));


            return $"{Convert.ToBase64String(salt)}${hashedPassword}";
        }

        /// <summary>
        /// Validate if password inserted by user is same as password in BD
        /// </summary>
        /// <param name="enteredPassword"></param>
        /// <param name="storedPassword"></param>
        /// <returns></returns>
        public bool ValidatePassword(string enteredPassword, string storedPassword)
        {
            return storedPassword == enteredPassword;
        }

        /// <summary>
        /// Generate a Salt to create a hash
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateSalt()
        {
            var salt = new byte[128 / 8]; // 128 bit salt
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }


       
    }
}
