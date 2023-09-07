using System;
using VerbTrainerAuth.Domain.Interfaces;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Infrastructure.Data.Repositories
{
	public class AsyncRecoveryTokenRepository : AsyncRepository<RecoveryToken>, IAsyncRecoveryTokenRepository
	{
        private readonly VerbTrainerAuthDbContext _dbContext;
        private readonly ILogger<AsyncRecoveryTokenRepository> _logger;

        public AsyncRecoveryTokenRepository(VerbTrainerAuthDbContext dbContext, ILogger<AsyncRecoveryTokenRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
    }
}

