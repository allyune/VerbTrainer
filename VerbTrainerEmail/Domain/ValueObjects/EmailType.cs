using System;
namespace VerbTrainerEmail.Domain.ValueObjects
{
	public enum EmailType
	{
		RegistrationConfirmationEmail = 1,
        PasswordResetEmail = 2,
		StreakReminder = 3,
		Invoice = 4,
		StatusChange = 5
	}
}

