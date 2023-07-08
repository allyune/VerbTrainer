using HebrewVerbs;
using Microsoft.EntityFrameworkCore;
using System.Resources;
using System.Xml.Serialization;
using VerbTrainer.Data;
using VerbTrainer.Models.Domain;

namespace VerbTrainer.DbInitializer
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
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();

			var grouped = _verbs.ListOfVerbs.Take(100).GroupBy(v => v.Binyan);

			var bi = 0;
			var vi = 0;
			foreach (var kvp in grouped)
			{
				_context.Binyanim.Add(new Binyan { Id = ++bi, Name = kvp.Key });

				foreach (var v in kvp)
					_context.Verbs.Add(
						new Models.Domain.Verb() { Id = ++vi, Name = v.Name, BinyanId = bi, Meaning = v.Meaning, Root = v.Root });

			}

			_context.SaveChanges();
		}
	}
}
