using System;
using VerbTrainerAuth.Domain.Base;

namespace VerbTrainerAuth.Domain.Entities
{
	public class RecoveryTokenEntity : BaseEntity
	{
        public string Token { get; private set; }
        public int UserId { get; private set; }
        public DateTime Validity { get; private set; }
        public bool Used { get; internal set; }

        private RecoveryTokenEntity(string token, int userId, DateTime validity, bool used)
        {
            Token = token;
            UserId = userId;
            Validity = validity;
            Used = used;
        }

        public static RecoveryTokenEntity CreateNew(string token, int userId, DateTime validity, bool used)
        {
            return new RecoveryTokenEntity(token, userId, validity, used);
        }
    }
}

