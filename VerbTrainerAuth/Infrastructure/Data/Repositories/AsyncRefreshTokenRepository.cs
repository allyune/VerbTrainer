using System;
using VerbTrainerAuth.Domain.Interfaces;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Infrastructure.Data.Repositories
{
    public class AsyncRefreshTokenRepository : AsyncRepository<RevokedRefreshToken>, IAsyncRefreshTokenRepository

    {
        private readonly VerbTrainerAuthDbContext _dbContext;
        private readonly ILogger<AsyncRefreshTokenRepository> _logger;

        public AsyncRefreshTokenRepository(VerbTrainerAuthDbContext dbContext, ILogger<AsyncRefreshTokenRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
    }
}

