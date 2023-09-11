using RazorLight;
//using Newtonsoft.Json;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Entities.UserEntity;
using VerbTrainerEmail.Domain.ValueObjects;
using VerbTrainerEmail.Infrastructure.Data.Models;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Application.Contracts.DTOs;
using VerbTrainerEmail.Domain.EmailTemplateModels;
using System.Text.Json;
using System.Text.Json.Serialization;

//TODO: Database table where each email request will be added immediately,
//and when email is sent -> marked sent
namespace VerbTrainerEmail.Application.Services.SendEmail
{
    public abstract class SendEmailService<T> : ISendEmailService<T> where T : EmailEntity, new()
    {

        public abstract T ParseJsonToEntity(string json);

        public T CreateEmailEntity(UserEntity userEntity)
        {
            T emailEntity = EmailEntity.CreateNew<T>(
                "TempFrom",
                userEntity.Id,
                userEntity.Email,
                userEntity.FirstName,
                EmailStatus.Draft,
                toUserLastName: userEntity.LastName);
            return emailEntity;
        }

        public Email CreateEmailDbModel(T emailEntity)
        {
            EmailType emailType;
            string entityType = emailEntity.GetType().Name;
            string? emailBody = emailEntity.Body?.Text;
            if (!Enum.TryParse(entityType, out emailType))
            {
                throw new ApplicationException(
                    $"Email type {entityType} not defined");
            }

            if (string.IsNullOrEmpty(emailBody))
            {
                throw new ArgumentException(
                    $"Email entity does not contain email body");
            }

            Email model = Email.CreateNew(
                emailType,
                emailEntity.From,
                emailEntity.ToUserId,
                emailEntity.Subject.Value,
                emailEntity.Body.Text,
                (int)emailEntity.Status);

            return model;
        }

        public async Task<string> RenderEmailTemplate(
            string templateFilePath,
            IEmailTemplateModel model,
            string? templateKey = null)
        {
            string template = File.ReadAllText(templateFilePath);

            if (string.IsNullOrEmpty(templateKey))
            {
                templateKey = Guid.NewGuid().ToString();
            }

            var engine = new RazorLightEngineBuilder()
                            .UseMemoryCachingProvider()
                            .Build();
            string result = await engine.CompileRenderStringAsync(templateKey, template, model);

            return result;
            
        }

    }
}

