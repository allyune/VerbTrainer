using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainerUser.Infrastructure.Data.Models
{
	public class RecoveryToken : BaseAuthModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string Token { get; private set; }
        public int? UserId { get; private set; }
        public User User { get; private set; }
        public DateTime Validity { get; private set; }
        public bool Used { get; set; }

        // TODO: Check if User must be provided
        private RecoveryToken(string token, int? userId, DateTime validity, bool used)
        {
            Token = token;
            UserId = userId;
            Validity = validity;
            Used = used;
        }

        public static RecoveryToken CreateNew(string token, int? userId, DateTime validity, bool used)
        {
            return new RecoveryToken(token, userId, validity, used);
        }
    }

    
}

