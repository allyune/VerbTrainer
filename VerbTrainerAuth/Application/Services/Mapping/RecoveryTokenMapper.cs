using System;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.DTOs;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.Services.Mapping
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

        public PasswordRecoveryRequestDto EntityToDto(RecoveryTokenEntity entity)
        {
            return PasswordRecoveryRequestDto.CreateNew(entity.UserId, entity.Token);
        }

        public RecoveryTokenEntity ModelToEntity(RecoveryToken model)
        {
            return RecoveryTokenEntity.CreateNew(
                model.Token, model.UserId, model.Validity, model.Used);
        }
    }
}

