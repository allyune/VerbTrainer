﻿using System;
using VerbTrainerUser.Domain.Entities;
using VerbTrainerUser.DTOs;
using VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Application.Services.Mapping
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

        public PasswordResetRequestDto EntityToDto(
            UserEntity user, RecoveryTokenEntity entity)
        {
            string link = "http://www.site.com/" + entity.Token;
            return PasswordResetRequestDto.CreateNew(
                user.Id,
                user.Email.EmailAddress,
                user.FirstName,
                (int)user.Status,
                user.LastLogin,
                link,
                user.LastName
                );
        }

        public RecoveryTokenEntity ModelToEntity(RecoveryToken model)
        {
            return RecoveryTokenEntity.CreateNew(
                model.Token, model.UserId, model.Validity, model.Used);
        }
    }
}

