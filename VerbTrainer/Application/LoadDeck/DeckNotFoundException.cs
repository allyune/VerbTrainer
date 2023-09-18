using System;
namespace VerbTrainer.Application.LoadDeck
{
	public class DeckNotFoundException : Exception
	{
		public DeckNotFoundException(string message) : base(message)
        {
		}
	}
}

