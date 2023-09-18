using System;
namespace VerbTrainer.Application.AddVerbToDeck
{
	public class RecordDoesNotExistsException : Exception
	{
		public RecordDoesNotExistsException(string message) : base(message)
        {
		}
	}
}

