using System;
namespace VerbTrainerUser.DTOs
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

