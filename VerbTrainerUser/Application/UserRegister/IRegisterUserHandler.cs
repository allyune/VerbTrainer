using System;
namespace VerbTrainerUser.Application.UserRegister
{
	public interface IRegisterUserHandler
	{
        public Task<bool> RegisterUser(
            string email, string password, string firstName);
    }
}

