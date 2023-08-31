using System;
using SharedUser = VerbTrainerSharedModels.Models.User;
using EmailUser = VerbTrainerEmail.Domain.Entities.User;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Application.User
{
	public static class UserMapper
	{
		public static EmailUser.User ModelToEntity(SharedUser.User userModel)
		{
			return EmailUser.User.CreateNew(userModel.Id,
											userModel.Email,
											userModel.FirstName,
											userModel.LastName,
											(UserStatus)userModel.Status,
											userModel.LastLogin);
		}
	}
}

