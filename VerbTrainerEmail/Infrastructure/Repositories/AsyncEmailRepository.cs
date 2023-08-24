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
        public AsyncEmailRepository(EmailDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
       
