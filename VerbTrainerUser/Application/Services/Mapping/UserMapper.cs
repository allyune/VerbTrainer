using System;
using VerbTrainerUser.Domain.Entities;
using VerbTrainerUser.Domain.ValueObjects;
using Models = VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Application.Services.Mapping
{
	public class UserMapper :IUserMapper
	{
        public UserEntity ModelToEntity(Models.User model)
        {
            return UserEntity.CreateNew(model.Email,
                                       (UserStatus)model.StatusCode,
                                       model.Password,
                                       model.Salt,
                                       model.Id,
                                       model.LastLogin,
                                       model.FirstName,
                                       model.LastName);
        }

        public Models.User EntityToModel(UserEntity entity)
        {
            return Models.User.CreateNew(
                entity.Email.EmailAddress,
                entity.Status,
                entity.Password.HashValue,
                entity.Password.Salt,
                entity.LastLogin,
                entity.FirstName,
                entity.LastName);
        }
    }
}

