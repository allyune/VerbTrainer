using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerbTrainerEmail.Domain;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Infrastructure.Data.Models
{
    public class Email
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set;  }
        public int Type { get; set; }
        public string From { get; private set; }
        public int ToUserId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public int Status { get; private set; }

        public ICollection<EmailAttachment>? Attachments { get; set; }
    }
}

