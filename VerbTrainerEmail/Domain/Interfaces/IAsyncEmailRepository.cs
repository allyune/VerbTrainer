using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Interfaces
{
	public interface IAsyncEmailRepository : IAsyncRepository<Email>
    {

	}
}

