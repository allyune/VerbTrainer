using System;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.DTOs
{
	public class VerbDto
	{
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Root { get; private set; }
        public string Meaning { get; private set; }
        public Binyan Binyan { get; private set; }
        public List<Conjugation> Conjugations { get; private set; }

        private VerbDto(
            int id,
            string name,
            string root,
            string meaning,
            Binyan binyan,
            List<Conjugation> conjugations)
        {
            Id = id;
            Name = name;
            Root = root;
            Meaning = meaning;
            Binyan = binyan;
            Conjugations = conjugations;
        }

        public static VerbDto CreateNew(
            int id,
            string name,
            string root,
            string meaning,
            Binyan binyan,
            List<Conjugation> conjugations)
        {
            return new VerbDto(
                id, name, root, meaning, binyan, conjugations);
        }
    }
}

