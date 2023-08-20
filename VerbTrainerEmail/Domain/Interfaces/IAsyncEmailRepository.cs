using System;
using VerbTrainerEmail.Domain.Base;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.ValueObjects;

namespace VerbTrainerEmail.Domain.Interfaces
{
	public interface IAsyncEmailRepository : IAsyncReadRepository<Email>, IAsyncWriteRepository<Email>
    {
		Task<EmailStatus> GetEmailStatus(Email email);
		Task UpdateEmailStatus(Email email, EmailStatus newStatus);
	}
}

