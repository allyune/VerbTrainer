using System;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Domain.Entities.User;

namespace VerbTrainerEmail.Application.User
{
	public class UserService : IUserService
    {

		private readonly IAsyncUserRepository _userRepository;

		public UserService(IAsyncUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Domain.Entities.User.User> GetUserById(Guid id)
		{
			// might return null = need a check for null-result at the caller
			return await _userRepository.GetAsync(id);

		}

    }

	public interface IUserService
	{
		Task<Domain.Entities.User.User> GetUserById(Guid id);
    }
}

