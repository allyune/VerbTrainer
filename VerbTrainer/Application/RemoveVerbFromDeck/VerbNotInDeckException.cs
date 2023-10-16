using System;
namespace VerbTrainer.Application.RemoveVerbFromDeck
{
	public class VerbNotInDeckException : Exception
	{
		public VerbNotInDeckException(string message) : base(message)
        {
		}
	}
}

