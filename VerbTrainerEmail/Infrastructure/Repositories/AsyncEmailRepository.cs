using System;
using VerbTrainerEmail.Infrastructure.Data;

namespace VerbTrainerEmail.Infrastructure
{
	public class AsyncEmailRepository
	{
		private readonly EmailDbContext _dbContext;
        ILogger<AsyncEmailRepository> _logger;

        public AsyncEmailRepository(EmailDbContext dbContext, ILogger<AsyncEmailRepository> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}


	}
}

