using System;
using VerbTrainerUser.Domain.Entities;
using VerbTrainerUser.DTOs;
using VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Application.Services.Mapping
{
	public interface IRecoveryTokenMapper
	{
		public RecoveryToken EntityToModel(RecoveryTokenEntity entity);
		public RecoveryTokenEntity ModelToEntity(RecoveryToken model);
		public PasswordResetRequestDto EntityToDto(
			UserEntity user, RecoveryTokenEntity entity);
    }
}

