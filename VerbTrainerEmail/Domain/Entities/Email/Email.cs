using System;
using System.Net.Mail;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
	public class Email
	{
        public Guid Id { get; private set; }
        public EmailType Type { get; private set; }
        public EmailSubject Subject { get; private set; }
        public EmailBody Body { get; private set; }
        public EmailStatus Status { get; private set; }
        public List<EmailAttachment>? Attachments { get; private set; }

        private Email(Guid id, EmailType type, EmailSubject subject, EmailBody body, List<EmailAttachment>? attachments)
        {
            Id = id;
            Type = type;
            Subject = subject;
            Body = body;
            Status = EmailStatus.Draft;
            Attachments = attachments;
        }

        public static Email CreateNew(EmailType type, EmailSubject subject, EmailBody body, List<EmailAttachment>? attachments = null)
        {
            var id = Guid.NewGuid();
            return new Email(id, type, subject, body, attachments);
        }
    }
}
