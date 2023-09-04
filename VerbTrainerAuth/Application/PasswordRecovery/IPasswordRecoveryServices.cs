using System;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.DTOs;
using Models = VerbTrainerAuth.Infrastructure.Data.Models;
namespace VerbTrainerAuth.Application.PasswordRecovery
{
	public interface IPasswordRecoveryServices
	{
		public RecoveryTokenEntity CreateToken(Models.User user);
		public Task<bool> ValidateRecoveryToken(string token, int userId);
		public Task SaveRecoveryToken(RecoveryTokenEntity token);
		public void RequestRecoveryEmail(PasswordRecoveryRequestDto requestDt);
	}
}

