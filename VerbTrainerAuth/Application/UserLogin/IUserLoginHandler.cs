using System;
namespace VerbTrainerAuth.Application.UserLogin
{
	public interface IUserLoginHandler
	{
		 Task<bool> UserLogin(
			string email,
            string password,
            string authHeader,
            IRequestCookieCollection cookies);
	}
}

