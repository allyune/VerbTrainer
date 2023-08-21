using System;
using System.Linq.Expressions;

namespace VerbTrainerEmail.Domain.Base
{
    public interface IAsyncReadOnlyRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);

        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);
    }

    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);

        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        Task<int> SaveChangesAsync();
    }

}  

