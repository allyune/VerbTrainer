using System;
using VerbTrainerAuth.Application.Services.Mapping;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.Domain.Interfaces;
using Models = VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.Services.User
{
	public class UserService : IUserService
	{
        private readonly IAsyncUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IUserMapper _mapper;

        public UserService(
            IAsyncUserRepository repository,
            ILogger<UserService> logger,
            IUserMapper mapper)
        {
            _userRepository = repository;
            _logger = logger;
            _mapper = mapper;

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

        public async Task<UserEntity> getUserInfoByEmail(string email)
        {
            Models.User? model = await _userRepository.GetAsync(e => e.Email == email);
            if (model is null)
            {
                throw new Exception("User not found in the database");
            }

            return _mapper.ModelToEntity(model);
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

        public async Task<int> AddUser(UserEntity user)
        {
            Models.User userModel = _mapper.EntityToModel(user);
            Console.WriteLine(userModel);
            Console.WriteLine(userModel.StatusCode);
            await _userRepository.AddAsync(userModel);
            return await _userRepository.SaveChangesAsync();
        }
    }
}

