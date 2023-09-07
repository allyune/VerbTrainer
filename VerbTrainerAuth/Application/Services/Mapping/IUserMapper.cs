using System;
using VerbTrainerAuth.Domain.Entities;
using Models = VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.Services.Mapping
{
	public interface IUserMapper
	{
		public UserEntity ModelToEntity(Models.User model);
        public Models.User EntityToModel(UserEntity model);
    }
}

