using System;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.DTOs
{
	public class VerbDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Root { get; set; }
        public string Meaning { get; set; }
        public Binyan Binyan { get; set; }
        public List<Conjugation> Conjugations { get; set; }
    }
}

