using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerbTrainerSharedModels.Models.User;

namespace VerbTrainer.Models.Domain
{
    public class Deck
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
                                        
        public User User { get; set; }

        public ICollection<DeckVerb>? DeckVerbs { get; set; }
    }
}