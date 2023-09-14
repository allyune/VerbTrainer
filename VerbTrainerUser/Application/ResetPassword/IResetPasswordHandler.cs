using System;
using VerbTrainerUser.Domain.Entities;
using VerbTrainerUser.DTOs;
using Models = VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Application.ResetPassword
{
	public interface IResetPasswordHandler
	{
		Task ResetPassword(string email);
        Task<bool> ValidateRecoveryToken(string token, int userId);
	}
}

