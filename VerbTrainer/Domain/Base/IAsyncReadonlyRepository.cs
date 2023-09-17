using System;
using System.Linq.Expressions;
using VerbTrainer.Infrastructure.Data.Models;

namespace VerbTrainer.Domain.Base
{
	public interface IAsyncReadonlyRepository<T> where T : BaseVerbTrainerModel
    {
        Task<T?> GetAsync(int id);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);
    }
}

