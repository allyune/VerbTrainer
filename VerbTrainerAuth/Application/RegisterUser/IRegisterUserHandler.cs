using System;
namespace VerbTrainerAuth.Application.RegisterUser
{
	public interface IRegisterUserHandler
	{
        public Task<bool> RegisterUser(
            string email, string password, string firstName);
    }
}

