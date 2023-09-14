using System;
namespace VerbTrainerUser.Application.UserLogin
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

