using System;
using VerbTrainerUser.Domain.Interfaces;
using VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Infrastructure.Data.Repositories
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

