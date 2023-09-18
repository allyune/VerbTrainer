using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainer.Infrastructure.Data.Models.Hebrew
{
	public class DeckVerb : BaseVerbTrainerModel
    {
        public int DeckId { get; private set; }
        public Deck Deck { get; private set; }

        public int VerbId { get; private set; }
        public Verb Verb { get; private set; }

        private DeckVerb(int deckId, int verbId)
        {
            DeckId = deckId;
            VerbId = verbId;
        }

        public static DeckVerb CreateNew(int deckId, int verbId)
        {
            return new DeckVerb(deckId, verbId);
        }

    }
}

