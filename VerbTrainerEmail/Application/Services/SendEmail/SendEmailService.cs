using RazorEngine.Templating;
using Newtonsoft.Json;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Entities.User;
using VerbTrainerEmail.Domain.ValueObjects;
using RazorEngine;
using Microsoft.Extensions.Hosting.Internal;
using VerbTrainerEmail.Infrastructure.Data.Models;
using System;

//TODO: Database table where each email request will be added immediately,
//and when email is sent -> marked sent
namespace VerbTrainerEmail.Application.Services.SendEmail
{
    public abstract class SendEmailService<T> : ISendEmailService<T> where T : EmailEntity, new()
    {

        public abstract
            Dictionary<string, object> CreateEmailModel(T emailEntity);


        public T CreateEmailEntity(
            Dictionary<string, object> emailData,
            User userEntity)
        {
            T emailEntity = EmailEntity.CreateNew<T>("TempFrom", userEntity.Id, userEntity.Email, (EmailStatus)emailData["Status"]);
            return emailEntity;
        }

        public Email CreateEmailDbModel(T emailEntity, string emailBody)
        {
            EmailType emailType;

            if (!Enum.TryParse(nameof(T), out emailType))
            {
                throw new ApplicationException($"Email type {nameof(T)} not defined");
            }

            Email model = Email.CreateNew(
                emailType,
                emailEntity.From,
                emailEntity.ToUserId,
                emailEntity.Subject.Value,
                emailBody,
                (int)emailEntity.Status);

            return model;
        }

        public T ParseJsonToEntity(string json)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (data is null ||
                   !data.ContainsKey("User") ||
                   !data.ContainsKey("Email"))
                {
                    //TODO: add handling
                    throw new ArgumentException("Invalid JSON format.");
                }

                Dictionary<string, object> userDict = (Dictionary<string, object>)data["User"];
                Dictionary<string, object> emailDict = (Dictionary<string, object>)data["Email"];

                User userEntity = User.CreateNew
                (
                    (int)userDict["Id"],
                    (string)userDict["Email"],
                    (string?)userDict["FirstName"],
                    (string?)userDict["LastName"],
                    (UserStatus)userDict["Status"],
                    (DateTime?)userDict["LastLogin"]
                );

                T emailEntity = CreateEmailEntity(emailDict, userEntity);

                return emailEntity;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error parsing JSON data.", ex);
            }

        }
        
        public string RenderEmailTemplate(T emailEntity)
        {
            var model = CreateEmailModel(emailEntity);
            var template = File.ReadAllText(emailEntity.Template);
            var result = Engine.Razor.RunCompile(template, null, model);
            return result;
        }

    }
}

