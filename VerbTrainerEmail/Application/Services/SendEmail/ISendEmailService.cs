using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Entities.UserEntity;
using VerbTrainerEmail.Infrastructure.Data.Models;

namespace VerbTrainerEmail.Application.Services.SendEmail
{
	public interface ISendEmailService<T> where T : EmailEntity
	{
        abstract T ParseJsonToEntity(string json);

        T CreateEmailEntity(UserEntity userEntity);

        abstract IEmailTemplateModel CreateEmailModel(
            T emailEntity,
            string passwordResetLink);

        Task <string> RenderEmailTemplate(
            T emailEntity,
            IEmailTemplateModel model,
            string? templatekey = null);

        Email CreateEmailDbModel(
            T emailEntity,
            string emailBody);

    }
}

