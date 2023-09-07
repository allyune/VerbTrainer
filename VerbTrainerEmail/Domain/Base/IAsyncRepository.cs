using System;
using System.Linq.Expressions;
using VerbTrainerEmail.Infrastructure.Data.Models;

namespace VerbTrainerEmail.Domain.Base
{

    public interface IAsyncRepository<T> where T : BaseEmailSenderModel
    {
        Task<T?> GetAsync(int id);

        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);

        Task AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        Task<int> SaveChangesAsync();
    }

}  

