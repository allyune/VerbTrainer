using System;
using VerbTrainerAuth.Domain.Entities;

namespace VerbTrainerAuth.Application.Services.User
{
	public interface IUserService
	{
		public Task<string?> GetEmailByUserId(int userId);
		public Task<int?> GetUserIdByEmail(string email);
		public Task<UserEntity> getUserInfoByEmail(string email);
		public Task<bool> CheckUserExists(string email);
		public Task<int> AddUser(UserEntity user);


    }
}

