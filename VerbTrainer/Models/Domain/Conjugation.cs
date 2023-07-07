using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VerbTrainer.Models.Domain
{
	public class Conjugation
    {

        [ForeignKey("VerbId")]
        public Verb Verb { get; set; }

        [ForeignKey("TenseId")]
        public Tense Tense { get; set; }

        public string Text { get; set; }
        public string Transcription { get; set; }
        public string Meaning { get; set; }


        public Guid VerbId { get; set; }
        public Guid TenseId { get; set; }
    }
}

