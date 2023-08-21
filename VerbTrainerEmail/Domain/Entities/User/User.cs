using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.User
{
	public class User : BaseEntity
	{
		public string Email { get; private set; }
		public string FirstName { get; private set; }
        public string LastName { get; private set; }
		public UserStatus Status { get; private set; }
		public DateTime? LastLogin { get; private set; }

		private User(Guid id, string email, string firstName,
					 string lastName, UserStatus status, DateTime? lastLogin)
		{
			Email = email;
			FirstName = firstName;
			LastName = lastName;
			Status = status;
			LastLogin = lastLogin;
        }

		public static User CreateNew(Guid id, string email, string firstName,
                     string lastName, UserStatus status, DateTime? lastLogin = null)
		{
			return new User(id, email, firstName, lastName, status, lastLogin);
		}
    }
}

