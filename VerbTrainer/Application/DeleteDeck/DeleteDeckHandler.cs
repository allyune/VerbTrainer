using System;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Application.LoadDeck;
using VerbTrainer.Domain.Interfaces;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.Application.DeleteDeck
{
    public interface IDeleteDeckHandler
    {
        Task DeleteDeckById(int id);
    }

    public class DeleteDeckHandler : IDeleteDeckHandler
    {
        private readonly ILogger<DeleteDeckHandler> _logger;
        private readonly IDeckRepository _deckRepository;

        public DeleteDeckHandler(
            ILogger<DeleteDeckHandler> logger,
            IDeckRepository deckRepository)
        {
            _logger = logger;
            _deckRepository = deckRepository;
        }

        public async Task DeleteDeckById(int id)
        {
            Deck? deck = await _deckRepository.GetAsync(d => d.Id == id);
            if (deck is null)
            {
                throw new DeckNotFoundException(
                $"Deck {id} not found.");
            }
            await _deckRepository.DeleteAsync(deck);
            int recordsDeleted = await _deckRepository.SaveChangesAsync();
            if (recordsDeleted == 0)
            {
                throw new CouldNotDeleteDeckException(
                    $"Deck ID {id} was not deleted");
            }
        }
    }
}

