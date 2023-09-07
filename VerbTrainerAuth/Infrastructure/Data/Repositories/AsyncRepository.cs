using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VerbTrainerAuth.Domain.Base;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Infrastructure.Data.Repositories
{
    public abstract class AsyncRepository<T> : IAsyncRepository<T> where T : BaseAuthModel
    {
        private readonly DbSet<T> _dbSet;
        private readonly VerbTrainerAuthDbContext _dbContext;

        public AsyncRepository(VerbTrainerAuthDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        Task<T?> IAsyncRepository<T>.GetAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        async Task<bool> IAsyncRepository<T>.CheckExists(Expression<Func<T, bool>> expression)
        {
            T? res = await _dbSet.FirstOrDefaultAsync(expression);
            if (res == null)
            {
                return false;
            }
            return true;
        }

        Task<List<T>> IAsyncRepository<T>.ListAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }

        async Task IAsyncRepository<T>.AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        Task<T> IAsyncRepository<T>.UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        async Task<int> IAsyncRepository<T>.SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        Task<bool> IAsyncRepository<T>.DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }
    }
}

