using System;
using VerbTrainerUser.Domain.Entities;
using Models = VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Application.Services.Mapping
{
	public interface IUserMapper
	{
		public UserEntity ModelToEntity(Models.User model);
        public Models.User EntityToModel(UserEntity model);
    }
}

