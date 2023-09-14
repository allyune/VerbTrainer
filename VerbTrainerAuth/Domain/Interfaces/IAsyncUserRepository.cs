using System;
using VerbTrainerUser.Domain.Base;
using VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Domain.Interfaces
{
    public interface IAsyncUserRepository : IAsyncRepository<User>
    {
    }
}

