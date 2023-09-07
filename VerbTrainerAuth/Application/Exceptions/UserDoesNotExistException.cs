using System;
namespace VerbTrainerAuth.Application.Exceptions
{
	public class UserDoesNotExistException : Exception
    {
		public UserDoesNotExistException(string message) : base(message)
        {
		}
	}
}
