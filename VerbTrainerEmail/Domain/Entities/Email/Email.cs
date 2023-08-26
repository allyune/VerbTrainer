using System;
using System.Net.Mail;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
    public class Email : BaseEntity
    {
        public int Id { get; private set; }
        public EmailType Type { get; private set; }
        public string From { get; private set; }
        public int ToUserId { get; private set; }
        public EmailSubject Subject { get; private set; }
        public EmailBody Body { get; private set; }
        public EmailStatus Status { get; private set; }
        public List<EmailAttachment>? Attachments { get; private set; }

        private Email(int id, EmailType type, int toUserId, EmailSubject subject, EmailBody body, List<EmailAttachment>? attachments)
        {
            Id = id;
            Type = type;
            ToUserId = toUserId;
            Subject = subject;
            Body = body;
            Status = EmailStatus.Draft;
            Attachments = attachments;
        }

        // creates an instance of persistent email (will be saved to the db)
        public static Email CreateWithEmailId(int id, EmailType type, int toUserId, EmailSubject subject, EmailBody body, List<EmailAttachment>? attachments = null)
        {
            if (Enum.IsDefined(typeof(PersistentEmailType), type))
            {
                return new Email(id, type, toUserId, subject, body, attachments);
            }
            throw new ArgumentException("Invalid email type: trying to create Persistent email of type", nameof(type));
        }

        //creates an instance of transient email (will be sent and discarded)
        public static Email CreateWithoutEmailId(EmailType type, int toUserId, EmailSubject subject, EmailBody body, List<EmailAttachment>? attachments = null)
        {

            if (Enum.IsDefined(typeof(TransientEmailType), type))
            {
                Random rand = new Random();
                return new Email(rand.Next(100000, 1000000), type, toUserId, subject, body, attachments);
            }
            throw new ArgumentException("Invalid email type: trying to create Transient email of type", nameof(type));
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
