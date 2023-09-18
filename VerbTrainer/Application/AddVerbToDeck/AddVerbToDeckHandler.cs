using System;
using HebrewVerbs;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Domain.Interfaces;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.Application.AddVerbToDeck
{
    public interface IAddVerbToDeckHandler
    {
        Task AddVerbToDeck(int verbId, int deckId);
    }

    public class AddVerbToDeckHandler : IAddVerbToDeckHandler
    {
        private readonly ILogger<AddVerbToDeckHandler> _logger;
        private readonly IDeckVerbRepository _deckVerbRepository;
        private readonly IDeckRepository _deckRepository;
        private readonly IVerbRepository _verbRepository;

        public AddVerbToDeckHandler(
            ILogger<AddVerbToDeckHandler> logger,
            IDeckVerbRepository deckVerbRepository,
            IDeckRepository deckRepository,
            IVerbRepository verbRepository)
        {
            _logger = logger;
            _deckVerbRepository = deckVerbRepository;
            _deckRepository = deckRepository;
            _verbRepository = verbRepository;
        }

        private async Task<bool> _checkVerbAndDeckExist(
            int verbId, int deckId)
        {
            bool deckExists = await _deckRepository
                .CheckRecordExists(d => d.Id == deckId);
            bool verbExists = await _verbRepository
                .CheckRecordExists(v => v.Id == verbId);
            if (!deckExists || !verbExists)
            {
                throw new RecordDoesNotExistsException(
                    $"deck status: {deckExists}, verb status {verbExists}");
            }
            return true;
        }

        private async Task<bool> _checkVerbDeckExists(
            int verbId, int deckId)
        {
            bool recordExists = await _deckVerbRepository.CheckRecordExists(
                dv => dv.DeckId == deckId && dv.VerbId == verbId);
            if (recordExists)
            {
                throw new DeckVerbAlreadyExistsException(
                    $"Verb {verbId} already linked to deck {deckId}");
            }
            return false;
        }

        public async Task AddVerbToDeck(
            int verbId, int deckId)
        {
            _ = await _checkVerbAndDeckExist(verbId, deckId);
            _ = await _checkVerbAndDeckExist(verbId, deckId);
            DeckVerb deckVerb = DeckVerb.CreateNew(deckId, verbId);
            await _deckVerbRepository.AddAsync(deckVerb);
            int recordsAdded = await _deckVerbRepository.SaveChangesAsync();
            if (recordsAdded == 0)
            {
                throw new AddVerbToDeckException(
                    $"Couldn't add verb {verbId} to deck {deckId}");
            }

        }
    }
}

