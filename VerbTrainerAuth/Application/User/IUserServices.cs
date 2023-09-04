using System;
using Models = VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.User
{
	public interface IUserServices
	{
		public Task<string?> GetEmailByUserId(int userId);
		public Task<int?> GetUserIdByEmail(string email);
		public Task<Models.User?> getUserInfoByEmail(string email);
		public Task<bool> CheckUserExists(string email);

	}
}

