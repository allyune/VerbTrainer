 using System;
using VerbTrainerAuth.Application.Exceptions;
using VerbTrainerAuth.Application.Services.User;
using VerbTrainerAuth.Domain.Entities;

namespace VerbTrainerAuth.Application.RegisterUser
{
	public class RegisterUserHandler : IRegisterUserHandler
    {

        private readonly IUserService _userService;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(
            IUserService userService,
            ILogger<RegisterUserHandler> logger)
        {
            _userService = userService;
            _logger = logger;
        }

		public async Task<bool> RegisterUser(string email, string password)
		{

            bool userExists = await _userService.CheckUserExists(email);

            if (userExists)
            {
                throw new UserAlreadyExistsException($"User with email {email} already exists");
            }

            UserEntity user = UserEntity.CreateNew(email, password);
            int saved = await _userService.AddUser(user);
            if (saved > 0)
            {
                return true;
            }
            return false;

        }
	
	}
}

