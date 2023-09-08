using System;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Entities.User;
using VerbTrainerEmail.Infrastructure.Data.Models;

namespace VerbTrainerEmail.Application.Services.SendEmail
{
	public interface ISendEmailService<T> where T : EmailEntity
	{
        T CreateEmailEntity(Dictionary<string, object> emailData, User userEntity);
        abstract Dictionary<string, object> CreateEmailModel(T emailEntity);
        T ParseJsonToEntity(string json);
        string RenderEmailTemplate(T emailEntity);
        Email CreateEmailDbModel(T emailEntity, string emailBody);

    }
}

