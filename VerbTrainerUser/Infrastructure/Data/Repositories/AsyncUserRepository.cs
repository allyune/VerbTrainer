using System;
using VerbTrainerUser.Domain.Interfaces;
using VerbTrainerUser.Infrastructure.Data.Repositories;
using VerbTrainerUser.Infrastructure.Data;
using VerbTrainerUser.Infrastructure.Data.Models;

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

