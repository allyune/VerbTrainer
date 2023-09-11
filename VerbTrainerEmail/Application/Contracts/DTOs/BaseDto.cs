using System;
namespace VerbTrainerEmail.Application.Contracts.DTOs
{
	public class BaseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? LastName { get; set; }
    }


}

