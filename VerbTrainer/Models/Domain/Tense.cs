using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainer.Models.Domain
{

	public class Tense
	{
		[Key]
        [Column(Order = 0)]
		public string Name { get; set; }

        public ICollection<Conjugation> Conjugations { get; set; }
    }
}

