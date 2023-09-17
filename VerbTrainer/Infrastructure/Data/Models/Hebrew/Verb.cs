using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainer.Infrastructure.Data.Models.Hebrew
{
	public class Verb
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Root { get; set; }
		public string Meaning { get; set; }
		public ICollection<Conjugation> Conjugations { get; set; }
        public ICollection<DeckVerb> DeckVerbs { get; set; }

        [ForeignKey("Binyan")]
		public int BinyanId { get; set; }
		public Binyan Binyan { get; set; }


	}

}