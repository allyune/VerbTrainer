using System;
using HebrewVerbs;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Application.SharedExceptions;
using VerbTrainer.Domain.Interfaces;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.Application.RemoveVerbFromDeck
{
    public interface IRemoveVerbFromDeckHandler
    {
        Task RemoveVerbFromDeck(int verbId, int deckId);
    }

    public class RemoveVerbFromDeckHandler : IRemoveVerbFromDeckHandler
    {
        private readonly ILogger<RemoveVerbFromDeckHandler> _logger;
        private readonly IDeckVerbRepository _deckVerbRepository;
        private readonly IDeckRepository _deckRepository;
        private readonly IVerbRepository _verbRepository;

        public RemoveVerbFromDeckHandler(
            ILogger<RemoveVerbFromDeckHandler> logger,
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

        public async Task RemoveVerbFromDeck(
            int verbId, int deckId)
        {
            _ = await _checkVerbAndDeckExist(verbId, deckId);
            DeckVerb? deckVerb = await _deckVerbRepository.GetAsync(
                dw => dw.DeckId == deckId && dw.VerbId == verbId);
            if (deckVerb is null)
            {
                throw new VerbNotInDeckException(
                    $"Verb {verbId} is not linked to deck {deckId}");
            }
            await _deckVerbRepository.DeleteAsync(deckVerb);
            int recordsDeleted = await _deckVerbRepository.SaveChangesAsync();
            if (recordsDeleted == 0)
            {
                throw new RemoveVerbFromDeckException(
                    $"Couldn't remove verb {verbId} from deck {deckId}. Try again.");
            }

        }
    }
}

