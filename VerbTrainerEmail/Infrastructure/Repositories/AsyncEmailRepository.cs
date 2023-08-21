using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Domain.ValueObjects;
using VerbTrainerEmail.Infrastructure.Data;
using VerbTrainerEmail.Infrastructure.Repositories;

namespace VerbTrainerEmail.Infrastructure
{
	public class AsyncEmailRepository : AsyncRepository<Email>, IAsyncEmailRepository
	{
		private readonly EmailDbContext _dbContext;
        private readonly ILogger<AsyncEmailRepository> _logger;
		private readonly DbSet<Email> _dbSet;

        public AsyncEmailRepository(EmailDbContext dbContext, ILogger<AsyncEmailRepository> logger)
            : base(dbContext)
        {
			_dbContext = dbContext;
            _dbSet = dbContext.Set<Email>();
            _logger = logger;
		}

        public async Task<EmailStatus> GetEmailStatus(Email email)
		{
            Guid id = email.Id;
            return await _dbSet
                .Where(e => e.Id == id)
                .Select(e => e.Status)
                .FirstAsync();
        }

        public async Task<List<Email>> ListEmailsByUserId(Guid userId)
        {
            return await _dbSet.Where(e => e.ToUserId == userId).ToListAsync();
        }

        //Task UpdateEmailStatus(Email email, EmailStatus newStatus)
        //{
        //    Guid id = email.Id;
        //    Email? dbEmail = _dbSet.FirstOrDefault(e => e.Id == id);
        //    EmailStatus currStatus = email.Status;
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
}
