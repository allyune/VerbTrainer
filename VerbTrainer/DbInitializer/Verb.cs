using System.Xml.Serialization;

namespace HebrewVerbs
{
	public class VerbConjugationVowelsEqualityComparer : IEqualityComparer<Verb>
	{
		private bool _onlyImportantTenses;

		public VerbConjugationVowelsEqualityComparer(bool onlyImportantTenses)
		{
			_onlyImportantTenses = onlyImportantTenses;
		}

		public bool Equals(Verb x, Verb y)
		{
			var diff = 0;
			foreach (var c in x.Tenses.Where(t =>
				!_onlyImportantTenses ||
				t.Type.ToString().StartsWith("AP") ||
				t.Type.ToString().StartsWith("PERF") ||
				t.Type.ToString().StartsWith("IMPF")))
			{
				Conjugation conjugation = y[c.Type.ToString()];
				if (conjugation != null)
				{
					if (conjugation.OnlyVowels != c.Conjugations[0].OnlyVowels)
					{
						diff++;

						if (diff == 4)
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		public int GetHashCode(Verb obj)
		{
			return 0;
		}
	}

	public class Verb
	{
		public string Name { get; set; }

		[XmlIgnore]
		public string NameWithoutVowels { get; set; }

		[XmlIgnore]
		public string OnlyVowels { get; set; }

		public int Rank { get; set; }

		public string Binyan { get; set; }
		public string Root { get; set; }
		public string Meaning { get; set; }
		public List<Tense> Tenses { get; set; }

		public Conjugation this[string tense]
		{
			get
			{
				var t = getTense(tense);
				if (t == null) return null;
				return t.Conjugations[0];
			}
		}

		public IEnumerable<Conjugation> GetConjugations()
		{
			return Tenses.SelectMany(t => t.Conjugations);
		}

		public Tense getTense(string name)
		{
			return Tenses.FirstOrDefault(t => t.Type.ToString().Equals(name));
		}

		public Tense getTense(TenseType type)
		{
			return Tenses.FirstOrDefault(t => t.Type.Equals(type));
		}

		public string ToCSV(bool isFull)
		{
			var res = Name + ", " + Root + ", " + Binyan + ", " + Meaning;
			if (isFull)
			{
				res += "\n" + string.Join("\n", GetConjugations().Select(c => c.Text + "\t" + FixTranscription(c.Transcription) + "\t" + c.Meaning));
			}

			return res;
		}

		private string FixTranscription(string transcription)
		{
			return transcription.Replace("***", "").Replace("^^^", "");
		}
	}
}
