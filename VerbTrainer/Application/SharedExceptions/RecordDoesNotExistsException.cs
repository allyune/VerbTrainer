using System;
namespace VerbTrainer.Application.SharedExceptions
{
	public class RecordDoesNotExistsException : Exception
	{
		public RecordDoesNotExistsException(string message) : base(message)
        {
		}
	}
}

