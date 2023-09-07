using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainerAuth.Infrastructure.Data.Models
{
	public class RevokedAccessToken : BaseAuthModel
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Token { get; set; }
    }
}
