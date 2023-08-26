using System;
namespace VerbTrainerEmail.Domain.ValueObjects
{
	public enum EmailType
	{
		RegistrationConfirmation,
		PasswordRecovery,
		StreakReminder,
		Invoice,
		StatusChange
	}
    public enum TransientEmailType
    {
        RegistrationConfirmation,
        PasswordRecovery,
        StreakReminder
    }
    public enum PersistentEmailType
    {
        Invoice,
        StatusChange
    }
}

