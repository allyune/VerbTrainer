using System;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain
{
	public class User
	{
		public Guid UserId { get; private set; }
		public string Email { get; private set; }
		public string FirstName { get; private set; }
        public string LastName { get; private set; }
		public UserStatus Status { get; private set; }
		public DateTime? LastLogin { get; private set; }

		public User()
		{
			
		}
	}
}

