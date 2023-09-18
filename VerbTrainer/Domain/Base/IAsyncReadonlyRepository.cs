using System;
using System.Linq.Expressions;
using VerbTrainer.Infrastructure.Data.Models;

namespace VerbTrainer.Domain.Base
{
	public interface IAsyncReadonlyRepository<T> where T : BaseVerbTrainerModel
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);
        Task<bool> CheckRecordExists(Expression<Func<T, bool>> expression);
    }
}

