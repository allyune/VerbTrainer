using System;
namespace VerbTrainerEmail.Application.SendPasswordResetEmail.Handler
{
	public interface IPasswordResetEmailHandler
	{
        Task SendPasswordResetEmail(string json);
    }
}

