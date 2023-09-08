using System;
using System.Net.Mail;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
    public abstract class EmailEntity
    {
        public string From { get; protected set; }
        public string ToUserEmail { get; protected set; }
        public int ToUserId { get; protected set; }
        public EmailStatus Status { get; protected set; }
        public List<EmailAttachment>? Attachments { get; protected set; }

        public abstract EmailSubject Subject { get; }
        public abstract EmailBody? Body { get; set; }
        public abstract string Template { get; }

        protected EmailEntity(
            string from,
            int toUserId,
            string toUserEmail,
            EmailStatus status,
            List<EmailAttachment>? attachments)
        {
            From = from;
            ToUserId = toUserId;
            ToUserEmail = toUserEmail;
            Status = status;
            Attachments = attachments;
        }

        public static T CreateNew<T> (
            string from,
            int toUserId,
            string toUserEmail,
            EmailStatus status,
            List<EmailAttachment>? attachments = null)
            where T : EmailEntity, new()
        {
            T emailEntity = new T();
            emailEntity.From = from;
            emailEntity.ToUserId = toUserId;
            emailEntity.ToUserEmail = toUserEmail;
            emailEntity.Status = status;
            emailEntity.Attachments = attachments;

            return emailEntity;
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