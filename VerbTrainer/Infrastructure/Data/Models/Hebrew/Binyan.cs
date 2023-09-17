using System;
using System.Collections.Generic;

namespace VerbTrainer.Infrastructure.Data.Models.Hebrew
{
    public class Binyan : BaseVerbTrainerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Verb> Verbs { get; set; }
    }
}