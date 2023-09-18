using System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VerbTrainer.Domain.Base;
using VerbTrainer.Infrastructure.Data;
using VerbTrainer.Infrastructure.Data.Models;

namespace VerbTrainer.Infrastructure.Repositories
{
	public abstract class AsyncReadonlyRepository<T> : IAsyncReadonlyRepository<T> where T : BaseVerbTrainerModel
    {
        private readonly DbSet<T> _dbSet;
        private readonly VerbTrainerDbContext _dbContext;

        public AsyncReadonlyRepository(VerbTrainerDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        public Task<List<T>> ListAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }

        public async Task<bool> CheckRecordExists(Expression<Func<T, bool>> expression)
        {
            return await GetAsync(expression) != null;
        }
    }
}

