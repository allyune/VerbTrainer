using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerbTrainer.Models.Domain;

namespace VerbTrainer.DTOs
{
	public class DeckDto
	{
        public string Name { get; set; }
        public List<VerbDto> Verbs { get; set; }
    }
}

