using System;
using System.Security.Cryptography;
using VerbTrainerAuth.Domain.Base;

namespace VerbTrainerAuth.Domain.Entities
{
	public class RecoveryTokenEntity : BaseEntity
	{
        public string Token { get; private set; }
        public int? UserId { get; private set; }
        public DateTime Validity { get; private set; }
        public bool Used { get; private set; }

        private RecoveryTokenEntity(
            string token, int? userId, DateTime validity, bool used)
        {
            Token = token;
            UserId = userId;
            Validity = validity;
            Used = used;
        }

        public static RecoveryTokenEntity CreateNew(int? userId)
        {
            // TODO: get rid of nullable userId
            if (userId is null)
            {
                throw new Exception("Trying to create a password reset token without user id");
            }
            const int tokenSize = 64;
            const int tokenValidityMinutes = 10;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(tokenSize);
            string tokenString = Convert.ToHexString(tokenBytes);

            return new RecoveryTokenEntity(
                tokenString,
                userId,
                DateTime.UtcNow.AddMinutes(tokenValidityMinutes),
                false);
        }

        public static RecoveryTokenEntity CreateNew(
            string token, int? userId, DateTime validity, bool used)
        {
            return new RecoveryTokenEntity(
                token, userId, validity, used);
        }

        public void SetUsed(bool value)
        {
            Used = value;
        }


    }
}

