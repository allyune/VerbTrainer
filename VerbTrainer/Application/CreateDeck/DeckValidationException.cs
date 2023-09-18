using System;

namespace VerbTrainer.Application.CreateDeck
{
	public class DeckValidationException : Exception
    {
		public DeckValidationException(string message) : base(message)
        {
		}
	}
}
