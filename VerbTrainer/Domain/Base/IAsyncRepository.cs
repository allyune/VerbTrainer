using System;
using System.Linq.Expressions;
using VerbTrainer.Infrastructure.Data.Models;

namespace VerbTrainer.Domain.Base
{
    public interface IAsyncRepository<T> where T : BaseVerbTrainerModel
    {
        Task<T?> GetAsync(int id);

        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);

        Task AddAsync(T model);

        Task<T> UpdateAsync(T model);

        Task<bool> DeleteAsync(T model);

        Task<int> SaveChangesAsync();
    }
}

