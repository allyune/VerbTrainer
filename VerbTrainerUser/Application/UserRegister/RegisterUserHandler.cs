 using System;
using VerbTrainerUser.Application.Exceptions;
using VerbTrainerUser.Application.Services.User;
using VerbTrainerUser.Domain.Entities;

namespace VerbTrainerUser.Application.UserRegister
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

		public async Task<bool> RegisterUser(
            string email, string password, string firstName)
		{

            bool userExists = await _userService.CheckUserExists(email);

            if (userExists)
            {
                throw new UserAlreadyExistsException($"User with email {email} already exists");
            }

            UserEntity user = UserEntity.CreateNew(0, email, password, firstName);
            int saved = await _userService.AddUser(user);
            if (saved > 0)
            {
                return true;
            }
            return false;

        }
	
	}
}

