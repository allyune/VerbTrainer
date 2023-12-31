﻿using System;
using System.Linq.Expressions;
using VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Domain.Base
{ 
    public interface IAsyncRepository<T> where T : BaseAuthModel
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);

        Task<bool> CheckExists(Expression<Func<T, bool>> expression);

        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);

        Task AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        Task<int> SaveChangesAsync();
    }

}

