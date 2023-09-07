using System;
using VerbTrainerAuth.Domain.Interfaces;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Infrastructure.Data.Repositories
{
	public class AsyncAccessTokenRepository : AsyncRepository<RevokedAccessToken>, IAsyncAccessTokenRepository

    {
        private readonly VerbTrainerAuthDbContext _dbContext;
        private readonly ILogger<AsyncAccessTokenRepository> _logger;

        public AsyncAccessTokenRepository(VerbTrainerAuthDbContext dbContext, ILogger<AsyncAccessTokenRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
    }
}

