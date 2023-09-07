using System;
using System.Text.RegularExpressions;
using VerbTrainerAuth.Domain.Base;
using VerbTrainerAuth.Domain.Exceptions;

namespace VerbTrainerAuth.Domain.ValueObjects
{
	public class Email : BaseValueObject
	{
        public string EmailAddress { get; private set; }

		private Email(string email)
		{
            EmailAddress = email;
		}

        public static Email CreateNew(string email)
        {
            // email follows correct format
            string emailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            bool emailValid = Regex.IsMatch(email, emailPattern);

            if (!emailValid)
            {

                throw new InvalidCredentialsException($"Email format is invalid");
            }

            return new Email(email);

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailAddress;
        }
    }
}

