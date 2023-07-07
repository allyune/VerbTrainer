using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainer.Models.Domain
{

	public class Tense
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

        public ICollection<Conjugation> Conjugations { get; set; }
    }
}


