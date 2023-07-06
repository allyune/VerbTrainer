﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainer.Models.Domain
{
	public class Verb
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Root { get; set; }
		public string Meaning { get; set; }
        public ICollection<Conjugation> Conjugations { get; set; }

        [ForeignKey("Binyan")]
        public Guid BinyanId { get; set; }
        public Binyan Binyan { get; set; }


    }

}