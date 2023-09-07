using System;
using System.Net.Mail;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
    public abstract class EmailEntity : BaseEntity
    {
        public string From { get; private set; }
        public string ToUserEmail { get; private set; }
        public EmailStatus Status { get; private set; }
        public List<EmailAttachment>? Attachments { get; private set; }

        public abstract EmailSubject Subject { get; set; }
        public abstract EmailBody? Body { get; set; }
        public abstract string Template { get; set; }

        public abstract EmailEntity CreateNew(
            string from,
            string ToUserEmail,
            EmailStatus status,
            List<EmailAttachment>? attachments);

        private EmailEntity(
            string from,
            string toUserEmail,
            EmailStatus status,
            List<EmailAttachment>? attachments)
        {
            From = from;
            ToUserEmail = toUserEmail;
            Status = status;
            Attachments = attachments;
        }

        public void SetStatus(EmailStatus newStatus)
        {
            if (!Enum.IsDefined(typeof(EmailStatus), newStatus))
            {
                throw new ArgumentException(
                    "Invalid email status value.", nameof(newStatus));
            }

            Status = newStatus;

        }
    }
}