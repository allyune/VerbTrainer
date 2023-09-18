using System;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Domain.Interfaces;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.Application.CreateDeck
{
	public interface ICreateDeckHandler
	{
		Task<int> CreateDeckForUser(int userId, string deckName);
	}

	public class CreateDeckHandler : ICreateDeckHandler
    {
		private readonly ILogger<CreateDeckHandler> _logger;
		private readonly IDeckRepository _deckRepository;

		public CreateDeckHandler(
			ILogger<CreateDeckHandler> logger,
			IDeckRepository deckRepository)
		{
			_logger = logger;
			_deckRepository = deckRepository;
		}

		public async Task<int> CreateDeckForUser(
			int userId, string deckName)
		{
			Deck newDeck = Deck.CreateNew(deckName, userId);
			await _deckRepository.AddAsync(newDeck);
            await _deckRepository.SaveChangesAsync();
			_logger.LogInformation
				($"Added deck {newDeck.Id} for user {userId}");
            return newDeck.Id;
        }
    }
}

