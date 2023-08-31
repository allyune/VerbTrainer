using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainerEmail.Infrastructure.Data.Models
{
	public class EmailAttachment : BaseEmailSenderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string FileName { get; set; }
        //TODO: make MimeType int => enum in entity
        public required string MimeType { get; set; }
        public required string Content { get; set; }
        public int EmailId { get; set; }
        public Email Email { get; set; }
    }
}

 