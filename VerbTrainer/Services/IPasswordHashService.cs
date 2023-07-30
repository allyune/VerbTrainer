using System;
namespace VerbTrainer.Services
{
	public interface IPasswordHashService
	{
		string HashPassword(string password, out string saltString);
		bool VerifyPasswordHash(string password, string savedHash, string salt);
	}
}

