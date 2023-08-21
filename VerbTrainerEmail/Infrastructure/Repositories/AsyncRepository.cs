using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Infrastructure.Data;

namespace VerbTrainerEmail.Infrastructure.Repositories
{
    public abstract class AsyncReadRepository<T> : IAsyncReadRepository<T> where T : BaseEntity
    {

        private readonly DbSet<T> _dbSet;

        public AsyncReadRepository(EmailDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }

        Task<T?> IAsyncReadRepository<T>.GetAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        Task<List<T>> IAsyncReadRepository<T>.ListAsync(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }
    }

    public abstract class AsyncWriteRepository<T> : IAsyncWriteRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet;
        private readonly EmailDbContext _dbContext;

        public AsyncWriteRepository(EmailDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }
    }
}

