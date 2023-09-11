using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Entities.UserEntity
{
	public class UserEntity
	{
		public int Id { get; private set; }
		public string Email { get; private set; }
		public string FirstName { get; private set; }
        public string? LastName { get; private set; }
		public UserStatus Status { get; private set; }
		public DateTime? LastLogin { get; private set; }

		private UserEntity(int id, string email, string firstName,
					 string? lastName, UserStatus status, DateTime? lastLogin)
		{
			Id = id;
			Email = email;
			FirstName = firstName;
			LastName = lastName;
			Status = status;
			LastLogin = lastLogin;
        }

		public static UserEntity CreateNew(int id, string email, string firstName,
			UserStatus status, DateTime? lastLogin = null, string? lastName = null)
		{
			return new UserEntity(id, email, firstName, lastName, status, lastLogin);
		}
    }
}

