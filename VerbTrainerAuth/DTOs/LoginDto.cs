using System;
namespace VerbTrainerAuth.DTOs
{
	public class LoginDto
	{
		public string email { get; set; }
		public string password { get; set; }
		public bool rememberUser { get; set; }
	}
}

