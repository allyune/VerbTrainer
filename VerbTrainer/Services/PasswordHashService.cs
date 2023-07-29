using System;
using System.Security.Cryptography;
using System.Text;

namespace VerbTrainer.Services
{
	public class PasswordHashService : IPasswordHashService
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        public string HashPassword(string password, out string saltString)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(keySize);
            saltString = Convert.ToHexString(salt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

    }
}

