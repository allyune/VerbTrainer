using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VerbTrainer.Models.Domain
{
	public class Conjugation
    {
        [Key, Column(Order = 0)]
        public Guid VerbId { get; set; }

        [Key, Column(Order = 1)]
        public Guid TenseId { get; set; }

        public string Text { get; set; }
        public string Transcription { get; set; }
        public string Meaning { get; set; }

        [ForeignKey("VerbId")]
        public Verb Verb { get; set; }

        [ForeignKey("TenseId")]
        public Tense Tense { get; set; }

    }
}

