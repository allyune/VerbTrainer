using System;
namespace VerbTrainer.Application.DeleteDeck
{
	public class CouldNotDeleteDeckException : Exception
    {
		public CouldNotDeleteDeckException(string message) : base(message)
        {
		}
	}
}