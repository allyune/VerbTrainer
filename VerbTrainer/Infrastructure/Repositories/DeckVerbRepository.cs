using System;
using VerbTrainer.Domain.Interfaces;
using VerbTrainer.Infrastructure.Data;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.Infrastructure.Repositories
{
    public class DeckVerbRepository : AsyncRepository<DeckVerb>, IDeckVerbRepository
    {
        private readonly VerbTrainerDbContext _dbContext;
        private readonly ILogger<DeckRepository> _logger;

        public DeckVerbRepository(
            VerbTrainerDbContext dbContext,
            ILogger<DeckRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
    }
}

