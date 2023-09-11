using System;
using VerbTrainerEmail.Application.Services.SendEmail;
using VerbTrainerEmail.Domain.Entities.Email;

namespace VerbTrainerEmail.Application.SendPasswordResetEmail
{
	public interface ISendPasswordResetEmail : ISendEmailService<PasswordResetEmail>
	{
	}
}

