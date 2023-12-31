﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerbTrainerEmail.Domain;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Infrastructure.Data.Models
{
    public class Email : BaseEmailSenderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public int Type { get; private set; }
        public string From { get; private set; }
        public int ToUserId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public int Status { get; private set; }

        public ICollection<EmailAttachment>? Attachments { get; set; }


        private  Email(int type, string from, int toUserId,
                      string subject, string body, int status)
        {
            Type = type;
            From = from;
            ToUserId = toUserId;
            Subject = subject;
            Body = body;
            Status = status;
       
        }

        public static Email CreateNew(EmailType type, string from, int toUserId,
                                      string subject, string body, int status)
        {
            return new Email((int)type, from, toUserId,
                             subject, body, status);
        }
    }
}