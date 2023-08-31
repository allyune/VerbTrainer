using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Infrastructure.Data;
using VerbTrainerEmail.Infrastructure.Data.Models;
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
       
