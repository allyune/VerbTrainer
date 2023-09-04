using System;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.Mapping
{
	public class RecoveryTokenMapper : IRecoveryTokenMapper
	{
        public RecoveryToken EntityToModel(RecoveryTokenEntity entity)
        {
            return RecoveryToken.CreateNew(entity.Token,
                                         entity.UserId,
                                         entity.Validity,
                                         entity.Used);
        }
        public RecoveryTokenEntity ModelToEntity(RecoveryToken model)
        {
            return RecoveryTokenEntity.CreateNew(model.Token, model.UserId, model.Validity, model.Used);
        }
    }
}

