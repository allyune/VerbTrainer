using System;
namespace VerbTrainerAuth.DTOs
{
	public class PasswordRecoveryRequestDto
	{
        public int? UserId { get; private set; }
        public string RecoveryToken { get; private set; }

        private PasswordRecoveryRequestDto(int? userId, string token)
        {
            UserId = userId;
            RecoveryToken = token;
        }

        public static PasswordRecoveryRequestDto CreateNew(int? userId, string token)
        {
            return new PasswordRecoveryRequestDto(userId, token);
        }
    }
}

