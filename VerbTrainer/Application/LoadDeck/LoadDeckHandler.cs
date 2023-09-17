using System;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Domain.Interfaces;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.Application.LoadDeck
{
    public interface ILoadDeckHandler
    {
        Task<Deck> LoadDeckById(int id);
    }

    public class LoadDeckHandler : ILoadDeckHandler
    {
        private readonly ILogger<LoadDeckHandler> _logger;
        private readonly IDeckRepository _deckRepository;

        public LoadDeckHandler(
            ILogger<LoadDeckHandler> logger,
            IDeckRepository deckRepository)
        {
            _logger = logger;
            _deckRepository = deckRepository;
        }

        public async Task<Deck> LoadDeckById(int id)
        {
            Deck? deck = await _deckRepository.GetAsync(d => d.Id == id);
            if (deck is not null)
            {
                return deck;
            }
            throw new DeckNotFoundException(
                $"Deck {id} not found.");
        }
    }
}

