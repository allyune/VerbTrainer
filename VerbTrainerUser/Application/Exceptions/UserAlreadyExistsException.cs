using System;
namespace VerbTrainerUser.Application.Exceptions
{
	public class UserAlreadyExistsException : Exception
    {
		public UserAlreadyExistsException(string message) : base(message)
        {
		}
	}
}
