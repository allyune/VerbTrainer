using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.DTOs
{
	public class DeckDto
	{
        public string Name { get; private set; }
        public List<VerbDto>? Verbs { get; private set; }

        private DeckDto(string name, List<VerbDto>? verbs)
        {
            Name = name;
            Verbs = verbs;
        }

        public static DeckDto CreateNew(
            string name,
            List<VerbDto>? verbs = null)
        {
            return new DeckDto(name, verbs);
        }
    }

    
}

