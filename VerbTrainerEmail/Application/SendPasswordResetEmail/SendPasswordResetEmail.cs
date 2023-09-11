using System;
using System.Text.Json;
using VerbTrainerEmail.Application.Contracts.DTOs;
using VerbTrainerEmail.Application.Services.SendEmail;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.EmailTemplateModels;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Entities.UserEntity;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Application.SendPasswordResetEmail
{
    public class SendPasswordResetEmail :
        SendEmailService<PasswordResetEmail>, ISendPasswordResetEmail
    {
        public override IEmailTemplateModel CreateEmailModel(
            PasswordResetEmail emailEntity,
            string passwordResetLink)
        {
            return PasswordResetModel.CreateNew(
                emailEntity.ToUserFirstName, passwordResetLink);
        }

        public override PasswordResetEmail ParseJsonToEntity(string json)
        {
            try
            {
                var data = JsonSerializer.Deserialize<ResetPasswordRequestDto>(json);

                if (data is null)
                {
                    throw new JsonException("JSON deserialize error: resut is null");
                }

                UserEntity userEntity = UserEntity.CreateNew(
                    data.UserId,
                    data.Email,
                    data.FirstName,
                    (UserStatus)data.Status,
                    data.LastLogin,
                    data.LastName);

                PasswordResetEmail emailEntity = CreateEmailEntity(userEntity);

                return emailEntity;
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Error parsing JSON data.", ex);
            }

        }
    }
}

