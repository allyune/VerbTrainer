using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerSharedModels.Models.User;
namespace VerbTrainerEmail.Domain.Interfaces
{
	public interface IAsyncUserRepository : IAsyncReadOnlyRepository<User>
    {
    }
}

