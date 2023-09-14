using System;
namespace VerbTrainerUser.Domain.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message)
        {
        }
    }

}

