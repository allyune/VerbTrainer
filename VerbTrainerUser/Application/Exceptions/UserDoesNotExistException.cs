using System;
namespace VerbTrainerUser.Application.Exceptions
{
	public class UserDoesNotExistException : Exception
    {
		public UserDoesNotExistException(string message) : base(message)
        {
		}
	}
}
