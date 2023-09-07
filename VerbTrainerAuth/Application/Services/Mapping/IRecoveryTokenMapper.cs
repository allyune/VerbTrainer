using System;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.DTOs;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.Services.Mapping
{
	public interface IRecoveryTokenMapper
	{
		public RecoveryToken EntityToModel(RecoveryTokenEntity entity);
		public RecoveryTokenEntity ModelToEntity(RecoveryToken model);
		public PasswordRecoveryRequestDto EntityToDto(RecoveryTokenEntity entity);
    }
}

