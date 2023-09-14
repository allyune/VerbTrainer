using System;
using VerbTrainerUser.Domain.Interfaces;
using VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Infrastructure.Data.Repositories
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

