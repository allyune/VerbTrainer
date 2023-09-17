using System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VerbTrainer.Domain.Base;
using VerbTrainer.Infrastructure.Data;
using VerbTrainer.Infrastructure.Data.Models;

namespace VerbTrainer.Infrastructure.Repositories
{
    public abstract class AsyncRepository<T> : AsyncReadonlyRepository<T>,
        IAsyncRepository<T> where T : BaseVerbTrainerModel
    {
        private readonly DbSet<T> _dbSet;
        private readonly VerbTrainerDbContext _dbContext;

        public AsyncRepository(VerbTrainerDbContext dbContext)
            : base(dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        async Task IAsyncRepository<T>.AddAsync(T model)
        {
            await _dbSet.AddAsync(model);
        }

        Task<T> IAsyncRepository<T>.UpdateAsync(T model)
        {
            _dbSet.Update(model);
            return Task.FromResult(model);
        }

        async Task<int> IAsyncRepository<T>.SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        Task<bool> IAsyncRepository<T>.DeleteAsync(T model)
        {
            _dbSet.Remove(model);
            return Task.FromResult(true);
        }
    }


}

