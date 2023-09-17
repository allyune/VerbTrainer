using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VerbTrainer.Infrastructure.Data.Models.Hebrew
{
    public class Conjugation : BaseVerbTrainerModel
    {
        [ForeignKey("VerbId")]
        public Verb Verb { get; set; }

        [ForeignKey("TenseId")]
        public Tense Tense { get; set; }

        public string Text { get; set; }
        public string Transcription { get; set; }
        public string Meaning { get; set; }


        public int VerbId { get; set; }
        public int TenseId { get; set; }
    }
}

