using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainer.Infrastructure.Data.Models.Hebrew
{
	public class DeckVerb
	{
        public int DeckId { get; set; }
        public Deck Deck { get; set; }

        public int VerbId { get; set; }
        public Verb Verb { get; set; }
    }
}

