using System;
namespace VerbTrainerAuth.DTOs
{
	public class IssueAccessTokenDto
	{
		public string Email { get; private set; }

        public IssueAccessTokenDto(string email)
        {
            Email = email;
        }
    }

	
}

