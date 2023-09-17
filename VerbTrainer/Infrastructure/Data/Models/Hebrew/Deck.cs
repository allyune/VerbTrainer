using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainer.Infrastructure.Data.Models.Hebrew
{
    public class Deck : BaseVerbTrainerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; private set; }
        public int UserId { get; private set; }

        public ICollection<DeckVerb>? DeckVerbs { get; set; }

        private Deck(string name, int userId)
        {
            Name = name;
            UserId = userId;
        }

        //Validation (e.g. name length) happens on DTO level
        public static Deck CreateNew(string name, int userId)
        {
            return new Deck(name, userId);
        }
    }
}