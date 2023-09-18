using System;
using VerbTrainer.Domain.Base;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.Domain.Interfaces
{
    public interface IDeckRepository : IAsyncRepository<Deck>
    {
    }
}


