using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.Entities.Email;

namespace VerbTrainerEmail.Domain.Interfaces
{
	public interface IAsyncUserRepository : IAsyncReadRepository<User>
    {
        Task<Email> ListEmails(User user);
    }
}

