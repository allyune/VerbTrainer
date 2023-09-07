using System;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.DTOs;
using Models = VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.ResetPassword
{
	public interface IResetPasswordHandler
	{
		Task ResetPassword(string email);
        Task<bool> ValidateRecoveryToken(string token, int userId);
	}
}

