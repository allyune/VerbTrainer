using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.DTOs
{
	public class DeckDto
	{
        public string Name { get; set; }
        public List<VerbDto> Verbs { get; set; }
    }
}

