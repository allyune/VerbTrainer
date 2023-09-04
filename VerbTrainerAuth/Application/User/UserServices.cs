using System;
using VerbTrainerAuth.Domain.Interfaces;
using Models = VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.User
{
	public class UserServices : IUserServices
	{
        private readonly IAsyncUserRepository _userRepository;
        private readonly ILogger<UserServices> _logger;

        public UserServices(IAsyncUserRepository repository, ILogger<UserServices> logger)
        {
            _userRepository = repository;
            _logger = logger;
        }

        public async Task<string?> GetEmailByUserId(int userId)
        {
            Models.User? user = await _userRepository.GetAsync(e => e.Id == userId);
            return user?.Email;
        }

        public async Task<int?> GetUserIdByEmail(string email)
        {
            Models.User? user = await _userRepository.GetAsync(e => e.Email == email);
            return user?.Id;
        }

        public async Task<Models.User?> getUserInfoByEmail(string email)
        {
            return await _userRepository.GetAsync(e => e.Email == email);
        }

        public async Task<bool> CheckUserExists(string email)
        {
            Models.User? user = await _userRepository.GetAsync(e => e.Email == email);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}

