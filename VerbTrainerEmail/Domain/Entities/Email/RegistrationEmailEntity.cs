using System;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
	public class RegistrationEmailEntity : EmailEntity
	{
        private EmailSubject RegistrationEmailSubject = EmailSubject.Create(
		"Get Started: Confirm Your New Account with VerbTrainer!");


        private string RegistrationEmailTemplate =
			"Domain/Templates/RegistrationConfirmationTemplate.cshtml";

        public override EmailSubject Subject => RegistrationEmailSubject;
        public override EmailBody? Body { get; set; }
		public override string Template => RegistrationEmailTemplate;

        public RegistrationEmailEntity(
        string from,
        int toUserId,
        string toUserEmail,
        EmailStatus status,
        List<EmailAttachment>? attachments)
        : base(from, toUserId, toUserEmail, status, attachments)
        {
        }

    }
}

