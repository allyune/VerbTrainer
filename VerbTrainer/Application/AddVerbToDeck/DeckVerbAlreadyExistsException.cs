using System;
namespace VerbTrainer.Application.AddVerbToDeck
{
	public class DeckVerbAlreadyExistsException : Exception
	{
		public DeckVerbAlreadyExistsException(string message) : base(message)
		{
		}
	}
}

