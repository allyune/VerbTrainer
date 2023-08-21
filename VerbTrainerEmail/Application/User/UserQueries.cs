using System;
using VerbTrainerEmail.Domain.Interfaces;

namespace VerbTrainerEmail.Application.User
{
	public class UserQueries : IUserQueries
	{

		private readonly IAsyncUserRepository _userRepository;

		public UserQueries(IAsyncUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

	}

	public interface IUserQueries
	{
        Task<IEnumerable<Domain.Entities.Author>> GetAllAsync();
    }
}

