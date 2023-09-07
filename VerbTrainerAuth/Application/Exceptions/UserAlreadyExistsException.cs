using System;
namespace VerbTrainerAuth.Application.Exceptions
{
	public class UserAlreadyExistsException : Exception
    {
		public UserAlreadyExistsException(string message) : base(message)
        {
		}
	}
}
