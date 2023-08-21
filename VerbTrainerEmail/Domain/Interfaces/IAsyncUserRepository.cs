using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Entities.User;

namespace VerbTrainerEmail.Domain.Interfaces
{
	public interface IAsyncUserRepository : IAsyncReadOnlyRepository<User>
    {
    }
}

