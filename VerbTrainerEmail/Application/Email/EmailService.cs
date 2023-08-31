using System;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.ValueObjects;
using System.Linq.Expressions;
using EmailModel = VerbTrainerEmail.Infrastructure.Data.Models;
using EmailEntity = VerbTrainerEmail.Domain.Entities.Email;

namespace VerbTrainerEmail.Application.Email
{
    public class EmailService : IEmailService
    {

        private readonly IAsyncEmailRepository _emailRepository;

        public EmailService(IAsyncEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task<EmailEntity.Email?> GetEmailById(int id)
        {
            
            EmailModel.Email? emailModel =  await _emailRepository.GetAsync(id);
            if (emailModel != null)
            {
                return EmailMapper.ModelToEntity(emailModel);
            }

            return null;
        }

        public async Task<EmailStatus?> GetEmailStatusAsync(int id)
        {
            EmailEntity.Email? email = await GetEmailById(id);
            if (email != null)
            {
                return email.Status;
            }
            return null;
        }

        public async Task<List<EmailEntity.Email>?> ListEmailsByUserId(int userId)
        {
            Expression<Func<EmailModel.Email, bool>> expression = email => email.ToUserId == userId;
            List<EmailModel.Email> emailModels = await _emailRepository.ListAsync(expression);
            if (emailModels.Count > 0)
            {
                List<EmailEntity.Email> res = emailModels.Select(EmailMapper.ModelToEntity).ToList();
            }
            return null;
        }


        public async Task WriteEmailToDb(EmailModel.Email email)
        {
           await _emailRepository.AddAsync(email);
        }

        //public async Task UpdateStatus(Email email, EmailStatus newStatus)
        //{
        //    Email? dbEmail = await GetEmailById(email.Id);
        //    EmailStatus currStatus = dbEmail.Status;
        //    if (dbEmail == null)
        //    {
        //        throw new ArgumentException("Email not found in the db", nameof(email));
        //    }
        //    else if (currStatus.Equals(newStatus))
        //    {
        //        //throw exception?
        //    }
        //    dbEmail.SetStatus(newStatus);

        //}

    }

    public interface IEmailService
    {
        Task<EmailEntity.Email?> GetEmailById(int id);
        Task<EmailStatus?> GetEmailStatusAsync(int id);
        Task<List<EmailEntity.Email>?> ListEmailsByUserId(int userId);
        Task WriteEmailToDb(EmailModel.Email email);
    }
}

