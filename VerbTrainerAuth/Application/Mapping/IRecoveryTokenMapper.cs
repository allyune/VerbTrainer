using System;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.Mapping
{
	public interface IRecoveryTokenMapper
	{
		public RecoveryToken EntityToModel(RecoveryTokenEntity entity);
		public RecoveryTokenEntity ModelToEntity(RecoveryToken model);
	}
}

