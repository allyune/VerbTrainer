using HebrewVerbs;
using System.Xml.Serialization;
using VerbTrainer.Infrastructure.Data;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;
using Conjugation = VerbTrainer.Infrastructure.Data.Models.Hebrew.Conjugation;
using Tense = VerbTrainer.Infrastructure.Data.Models.Hebrew.Tense;
using Verb = VerbTrainer.Infrastructure.Data.Models.Hebrew.Verb;

namespace VerbTrainer.Infrastructure.DbInitializer
{
    public class DbInitializer
	{
		private VerbTrainerDbContext _context;
		private Verbs? _verbs;

		public DbInitializer(VerbTrainerDbContext context)
		{
			_context = context;

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Verbs));
			using (Stream stream = GetType().Assembly.
					   GetManifestResourceStream("VerbTrainer.allverbs"))
			{
				_verbs = (Verbs)xmlSerializer.Deserialize(stream);
			}
		}

		public void Run()
		{
			//_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();

			var ti = 0;
			foreach (var tt in Enum.GetNames(typeof(TenseType)))
			{
				_context.Tenses.Add(new Tense() { Id = ++ti, Name = tt });
			}

			var grouped = _verbs.ListOfVerbs.Take(100).GroupBy(v => v.Binyan);

			var bi = 0;
			var vi = 0;
			foreach (var kvp in grouped)
			{
				_context.Binyanim.Add(new Binyan { Id = ++bi, Name = kvp.Key });

				foreach (var v in kvp)
				{
					_context.Verbs.Add(
						new Verb() { Id = ++vi, Name = v.Name, BinyanId = bi, Meaning = v.Meaning, Root = v.Root });

					foreach (var t in v.Tenses)
					{
						_context.Conjugations.Add(
							t.Conjugations.Select(
								c => new Conjugation()
								{
									VerbId = vi,
									Text = c.Text,
									Transcription = c.Transcription,
									Meaning = c.Meaning,
									TenseId = ((int)t.Type) + 1
								}).First());

					}
				}
			}

            _context.SaveChanges();

            _context.Decks.Add(Deck.CreateNew("Top 100 Hebrew Verbs", 1));
            _context.SaveChanges();
			Deck deck = _context.Decks.First(d => d.Name == "Top 100 Hebrew Verbs");

            List<Verb> top100 = _context.Verbs.ToList();

			top100.ForEach(v => _context.DeckVerbs.Add(DeckVerb.CreateNew(deck.Id, v.Id)));

            _context.SaveChanges();
		}
	}
}
