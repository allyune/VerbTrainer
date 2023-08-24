using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainerEmail.Infrastructure.Data.Models
{
	public class EmailAttachment
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Content { get; set; }
        public int EmailId { get; set; }
        public Email Email { get; set; }
    }
}

