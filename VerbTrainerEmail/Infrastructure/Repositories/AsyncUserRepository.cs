﻿using System;
using VerbTrainerSharedModels.Models.User;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Infrastructure.Data;

namespace VerbTrainerEmail.Infrastructure.Repositories
{

	public class AsyncUserRepository : AsyncReadOnlyRepository<User>, IAsyncUserRepository
    {
        private readonly EmailDbContext _dbContext;
        private readonly ILogger<AsyncUserRepository> _logger;

        public AsyncUserRepository(EmailDbContext dbContext, ILogger<AsyncUserRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


    }
}

