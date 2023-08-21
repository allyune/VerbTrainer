using System;
using System.Linq.Expressions;

namespace VerbTrainerEmail.Domain.Base
{
    public interface IAsyncReadRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);

        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);
    }

    public interface IAsyncWriteRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        Task<int> SaveChanges();
    }

}  

