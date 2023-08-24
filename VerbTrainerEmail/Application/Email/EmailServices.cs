using System;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.ValueObjects;
using System.Linq.Expressions;

namespace VerbTrainerEmail.Application.User
{
    public class EmailService : IEmailService
    {

        private readonly IAsyncEmailRepository _emailRepository;

        public EmailService(IAsyncEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task<Email> GetEmailById(Guid id)
        {
            // might return null = need a check for null-result at the caller
            return await _emailRepository.GetAsync(id);
        }

        public async Task<EmailStatus> GetEmailStatusAsync(Guid id)
        {
            Email email = await GetEmailById(id);
            return email.Status;
        }

        public async Task<List<Email>> ListEmailsByUserId(Guid userId)
        {
            Expression<Func<Email, bool>> expression = email => email.Id == userId;
            return await _emailRepository.ListAsync(expression);
        }


        public async Task SaveEmail(Email email)
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
        Task<Email> GetEmailById(Guid id);
        Task<EmailStatus> GetEmailStatusAsync(Guid id);
        Task<List<Email>> ListEmailsByUserId(Guid userId);
        Task SaveEmail(Email email);
    }
}

