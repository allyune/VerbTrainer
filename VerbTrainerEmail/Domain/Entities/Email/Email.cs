using System;
using System.Net.Mail;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
    public class Email : BaseEntity
    {
        public EmailType Type { get; private set; }
        public string From { get; private set; }
        public Guid ToUserId { get; private set; }
        public EmailSubject Subject { get; private set; }
        public EmailBody Body { get; private set; }
        public EmailStatus Status { get; private set; }
        public List<EmailAttachment>? Attachments { get; private set; }

        private Email(Guid id, EmailType type, Guid toUserId, EmailSubject subject, EmailBody body, List<EmailAttachment>? attachments)
        {
            Id = id;
            Type = type;
            ToUserId = toUserId;
            Subject = subject;
            Body = body;
            Status = EmailStatus.Draft;
            Attachments = attachments;
        }

        public static Email CreateNew(EmailType type, Guid toUserId, EmailSubject subject, EmailBody body, List<EmailAttachment>? attachments = null)
        {
            var id = Guid.NewGuid();
            return new Email(id, type, toUserId, subject, body, attachments);
        }

        public void SetStatus(EmailStatus newStatus)
        {
            if (!Enum.IsDefined(typeof(EmailStatus), newStatus))
            {
                throw new ArgumentException("Invalid email status value.", nameof(newStatus));
            }

            this.Status = newStatus;
        }
    }
}
