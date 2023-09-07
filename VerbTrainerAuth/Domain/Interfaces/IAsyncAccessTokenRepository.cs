using System;
using VerbTrainerAuth.Domain.Base;
using Models = VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Domain.Interfaces
{
	public interface IAsyncAccessTokenRepository : IAsyncRepository<Models.RevokedAccessToken>
    {
	}
}

