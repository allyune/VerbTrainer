using System;
using VerbTrainerEmail.Domain.ValueObjects;
using EmailModel = VerbTrainerEmail.Infrastructure.Data.Models;
using EmailEntity = VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain;

namespace VerbTrainerEmail.Application.Email
{
	public static class EmailMapper
	{
        public static EmailEntity.Email ModelToEntity(EmailModel.Email email)
        {
            ICollection<EmailModel.EmailAttachment>? modelAttachments = email.Attachments;
            List<EmailAttachment>? entityAttachments = new List<EmailAttachment> { };
            if (modelAttachments != null)
            {
                entityAttachments = modelAttachments
                                    .Select(a => EmailAttachment.Create(a.FileName,
                                                                        a.MimeType,
                                                                        Convert.FromBase64String(a.Content)))
                                    .ToList();
            }
            else
            {
                entityAttachments = null;
            }

            return EmailEntity.Email
                   .CreateWithEmailId(email.Id,
                                     (EmailType)email.Type,
                                     email.ToUserId,
                                     EmailSubject.Create(email.Subject),
                                     EmailBody.Create(email.Body),
                                     entityAttachments);

        }
    }
}

