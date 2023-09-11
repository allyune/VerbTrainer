using System;
using System.Reflection;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
    public class RegistrationEmail : EmailEntity
	{
        private EmailSubject RegistrationEmailSubject = EmailSubject.Create(
		"Get Started: Confirm Your New Account with VerbTrainer!");

        private string ConstructTemplatePath()
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            string templateFolderPath = Path.Combine(
                baseDirectory, "Domain", "Templates");
            string templateFilePath = Path.Combine(
                templateFolderPath, "RegistrationConfirmationTemplate.cshtml");
            return templateFilePath;
        }

        private string RegistrationEmailTemplate =
			"Domain/Templates/RegistrationConfirmationTemplate.cshtml";

        public override EmailSubject Subject => RegistrationEmailSubject;
        public override EmailBody? Body { get; set; }
		public override string Template => ConstructTemplatePath();

        //to satisfy a condition of genenric class SendEmail of T having an
        //parameterless constructor
        public RegistrationEmail()
        {
        }

        protected RegistrationEmail(
        string from,
        int toUserId,
        string toUserFirstName,
        string toUserLastName,
        string toUserEmail,
        EmailStatus status,
        List<EmailAttachment>? attachments)
        : base(
              from,
              toUserId,
              toUserFirstName,
              toUserLastName,
              toUserEmail,
              status,
              attachments)
        {
        }

  
      

    }
}

