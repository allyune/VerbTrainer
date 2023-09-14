using System;
using VerbTrainerUser.Domain.Base;
using Models = VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Domain.Interfaces
{
	public interface IAsyncAccessTokenRepository : IAsyncRepository<Models.RevokedAccessToken>
    {
	}
}

