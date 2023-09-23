using System;
namespace VerbTrainer.Application.SharedExceptions
{
	public class DeckVerbAlreadyExistsException : Exception
	{
		public DeckVerbAlreadyExistsException(string message) : base(message)
		{
		}
	}
}

