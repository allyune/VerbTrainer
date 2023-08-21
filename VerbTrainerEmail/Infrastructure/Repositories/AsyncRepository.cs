using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Infrastructure.Data;

namespace VerbTrainerEmail.Infrastructure.Repositories
{
    public abstract class AsyncReadOnlyRepository<T> : IAsyncReadOnlyRepository<T> where T : BaseEntity
    {

        private readonly DbSet<T> _dbSet;

        public AsyncReadOnlyRepository(EmailDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }

        Task<T?> IAsyncReadOnlyRepository<T>.GetAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        Task<List<T>> IAsyncReadOnlyRepository<T>.ListAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }
    }

    public abstract class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet;
        private readonly EmailDbContext _dbContext;

        public AsyncRepository(EmailDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        Task<T?> IAsyncRepository<T>.GetAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        Task<List<T>> IAsyncRepository<T>.ListAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }

        async Task<T> IAsyncRepository<T>.AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
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

