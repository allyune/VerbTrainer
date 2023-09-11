using System;
using System.Reflection;
using VerbTrainerEmail.Application.Contracts.DTOs;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
    public class PasswordResetEmail : EmailEntity
    {
        private EmailSubject RegistrationEmailSubject = EmailSubject.Create(
        "Your VerbTrainer password reset link");

        public override EmailSubject Subject => RegistrationEmailSubject;
        public override EmailBody? Body { get; set; }
        public override string Template => ConstructTemplatePath();

        private string ConstructTemplatePath()
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            string templateFolderPath = Path.Combine(
                baseDirectory, "Domain", "Templates");
            string templateFilePath = Path.Combine(
                templateFolderPath, "PasswordResetTemplate.cshtml");
            return templateFilePath;
        }

        //to satisfy a condition of genenric class SendEmail of T having an
        //parameterless constructor
        public PasswordResetEmail()
        {
        }

        protected PasswordResetEmail(
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

