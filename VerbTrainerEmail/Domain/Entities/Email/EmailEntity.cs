using System;
using System.Net.Mail;
using VerbTrainerEmail.Application.Contracts.DTOs;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.Email
{
    public abstract class EmailEntity
    {
        public string From { get; protected set; }
        public string ToUserEmail { get; protected set; }
        public int ToUserId { get; protected set; }
        public string ToUserFirstName { get; protected set; }
        public string? ToUserLastName { get; protected set; }
        public EmailStatus Status { get; protected set; }
        public List<EmailAttachment>? Attachments { get; protected set; }

        public abstract EmailSubject Subject { get; }
        public abstract EmailBody? Body { get; set; }
        public abstract string Template { get; }

        //to satisfy a condition of genenric class SendEmail of T having an
        //parameterless constructor
        public EmailEntity()
        {
        }

        protected EmailEntity(
            string from,
            int toUserId,
            string toUserFirstName,
            string toUserLastName,
            string toUserEmail,
            EmailStatus status,
            List<EmailAttachment>? attachments)
        {
            From = from;
            ToUserId = toUserId;
            ToUserFirstName = toUserFirstName;
            ToUserLastName = toUserLastName;
            ToUserEmail = toUserEmail;
            Status = status;
            Attachments = attachments;
        }

        public static T CreateNew<T> (
            string from,
            int toUserId,
            string toUserEmail,
            string toUserFirstName,
            EmailStatus status,
            List<EmailAttachment>? attachments = null,
            string? toUserLastName = null)
            where T : EmailEntity, new()
        {
            T emailEntity = new T();
            emailEntity.From = from;
            emailEntity.ToUserId = toUserId;
            emailEntity.ToUserFirstName = toUserFirstName;
            emailEntity.ToUserLastName = toUserLastName;
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