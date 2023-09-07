using System;
using VerbTrainerAuth.Domain.Interfaces;
using VerbTrainerAuth.Infrastructure.Data.Repositories;
using VerbTrainerAuth.Infrastructure.Data;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerEmail.Infrastructure.Repositories
{

    public class AsyncUserRepository : AsyncRepository<User>, IAsyncUserRepository
    {
        private readonly VerbTrainerAuthDbContext _dbContext;
        private readonly ILogger<AsyncUserRepository> _logger;

        public AsyncUserRepository(VerbTrainerAuthDbContext dbContext, ILogger<AsyncUserRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


    }
}

