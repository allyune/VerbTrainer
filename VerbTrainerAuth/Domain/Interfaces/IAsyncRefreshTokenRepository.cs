using System;
using VerbTrainerAuth.Domain.Base;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Domain.Interfaces
{
    public interface IAsyncrefreshTokenRepository : IAsyncRepository<RevokedAccessToken>
    {
    }
}
