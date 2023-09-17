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

        Task<T?> IAsyncReadonlyRepository<T>.GetAsync(int id)
        {
            return _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        Task<List<T>> IAsyncReadonlyRepository<T>.ListAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }
    }
}

