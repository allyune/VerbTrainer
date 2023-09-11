using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using VerbTrainerEmail.Application.Contracts.DTOs;
using VerbTrainerEmail.Domain.EmailTemplateModels;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Domain.ValueObjects;
using VerbTrainerEmail.Infrastructure.Data.Models;

namespace VerbTrainerEmail.Application.SendPasswordResetEmail.Handler
{
	public class PasswordResetEmailHandler : IPasswordResetEmailHandler
    {
		private readonly ISendPasswordResetEmail _emailSender;
        private readonly IAsyncEmailRepository _emailRepository;

		public PasswordResetEmailHandler(
            ISendPasswordResetEmail emailSender,
            IAsyncEmailRepository emailRepository
            )
		{
			_emailSender = emailSender;
            _emailRepository = emailRepository;
        }

        private string ParseResetLink(string json)
        {
            try
            {
                var data = JsonConvert
                    .DeserializeObject<ResetPasswordRequestDto>(json);

                if (data is null)
                {
                    throw new JsonException(
                        "JSON deserialize error: resut is null");
                }

                return data.ResetLink;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Error parsing JSON data.", ex);
            }

        }

        //TODO: handle attachments
        public async Task SendPasswordResetEmail(string json)
		{
            Console.WriteLine("Started email handling");
			PasswordResetEmail emailEntity =
                _emailSender.ParseJsonToEntity(json);

            string resetLink = ParseResetLink(json);

            PasswordResetModel model = PasswordResetModel.CreateNew(
				emailEntity.ToUserFirstName, resetLink);

            string emailBody = await _emailSender
                .RenderEmailTemplate(emailEntity, model);

            emailEntity.SetStatus(EmailStatus.Sent);

            Email emailModel = _emailSender
                .CreateEmailDbModel(emailEntity, emailBody);

            await _emailRepository.AddAsync(emailModel);
            await _emailRepository.SaveChangesAsync();

        }
	}
}

