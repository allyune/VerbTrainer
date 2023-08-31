using System;
using System.Collections.Generic;

namespace VerbTrainer.Models.Domain
{
    public class Binyan
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Verb> Verbs { get; set; }
    }
}