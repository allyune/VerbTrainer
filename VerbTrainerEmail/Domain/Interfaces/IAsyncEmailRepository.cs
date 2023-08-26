using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Infrastructure.Data.Models;

namespace VerbTrainerEmail.Domain.Interfaces
{
	public interface IAsyncEmailRepository : IAsyncRepository<Email>
    {

	}
}

