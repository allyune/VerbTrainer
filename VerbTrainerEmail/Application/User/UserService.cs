using System;
using VerbTrainerEmail.Domain.Interfaces;
using SharedUser = VerbTrainerSharedModels.Models.User;
using EmailUser = VerbTrainerEmail.Domain.Entities.User;

namespace VerbTrainerEmail.Application.User
{
	public class UserService : IUserService
    {

		private readonly IAsyncUserRepository _userRepository;

		public UserService(IAsyncUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		// Returns a User Domain entity created from User model.
		public async Task<EmailUser.User?> GetUserById(int id)
		{
			SharedUser.User? userModel = await _userRepository.GetAsync(id);
            if (userModel != null)
            {
                return UserMapper.ModelToEntity(userModel);
            }

            return null;
        }

    }

	public interface IUserService
	{
		Task<EmailUser.User?> GetUserById(int id);
    }
}

