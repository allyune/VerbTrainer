using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Infrastructure.Data;
using VerbTrainerEmail.Infrastructure.Data.Models;

    public abstract class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEmailSenderModel
    {
        private readonly DbSet<T> _dbSet;
        private readonly EmailDbContext _dbContext;

        public AsyncRepository(EmailDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        Task<T?> IAsyncRepository<T>.GetAsync(int id)
        {
            return _dbSet.FirstOrDefaultAsync(e => e.Id == id);
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


