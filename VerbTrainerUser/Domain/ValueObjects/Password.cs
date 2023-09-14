using System;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using VerbTrainerUser.Domain.Base;
using VerbTrainerUser.Domain.Exceptions;

namespace VerbTrainerUser.Domain.ValueObjects
{
	public class Password : BaseValueObject
	{
		public string HashValue { get; private set; }
        public string Salt { get; }

        private Password(string hashValue, string salt)
        {
            HashValue = hashValue;
            Salt = salt;
            
        }

        private static string HashPassword(
            string password,
            out string saltString,
            string? savedSalt = null)
        {
                HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
                const int keySize = 64;
                const int iterations = 350000;
                byte[] salt;
                if (savedSalt is null)
                {
                    salt = RandomNumberGenerator.GetBytes(keySize);
                    saltString = Convert.ToHexString(salt);
            }
                else
                {
                    salt = Convert.FromHexString(savedSalt);
                    saltString = savedSalt;
                }

                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    iterations,
                    hashAlgorithm,
                    keySize);
                return Convert.ToHexString(hash);
            
        }

        private static bool IsPasswordFormatValid(string passwordRawString)
        {
            string validPasswordPattern = @"^(?=.*\d)(?=.*[!@#$%^&*()])(?=.*[A-Z])(?=.*[a-z]).{8,}$";
            return Regex.IsMatch(passwordRawString, validPasswordPattern);
        }

        // creates an instance of password with password hash string
        public static Password CreateNew(string passwordHash, string salt)
        {
            return new Password(passwordHash, salt);
        }

        // creates an instance of the password, including hashing from the raw password string
        // used when creating new users.
		public static Password CreateNew(string passwordRawString)
		{
            Console.WriteLine(passwordRawString);
            bool passwordValid = IsPasswordFormatValid(passwordRawString);
            if (!passwordValid)
            {
                throw new InvalidCredentialsException($"Password format is invalid");
            }

            string hashedPassword = HashPassword(passwordRawString,
                                                 out string saltString);

            return new Password(hashedPassword, saltString);

        }

        internal bool CompareTo(string rawPassword)
        {
           string providedHash = HashPassword(rawPassword,
                                              out string saltString,
                                              this.Salt);
            Console.WriteLine(rawPassword);
            Console.WriteLine(HashValue);
            Console.WriteLine(providedHash);
            return CryptographicOperations
                    .FixedTimeEquals(Convert.FromHexString(providedHash),
                                     Convert.FromHexString(this.HashValue));
        }

        internal void UpdateValue(string newPasswordRawString)
        {
            bool passwordValid = IsPasswordFormatValid(newPasswordRawString);
            if (!passwordValid)
            {
                throw new InvalidCredentialsException($"Password format is invalid");
            }

            string newPasswordHash = HashPassword(newPasswordRawString,
                                                  out string saltString,
                                                  Salt);
            HashValue = newPasswordHash;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return HashValue;
        }

    }
}

