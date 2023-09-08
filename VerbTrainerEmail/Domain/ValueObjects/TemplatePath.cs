using System;
using VerbTrainerEmail.Domain.Base;
using static System.Net.Mime.MediaTypeNames;

namespace VerbTrainerEmail.Domain.ValueObjects
{
	public class TemplatePath : BaseValueObject
	{
		public string PathString { get; private set; }

		private TemplatePath(string path)
		{
			PathString = path;
		}

		public static TemplatePath CreateNew(string relativePath)
		{
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

			return new TemplatePath(templateFilePath);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PathString;
        }
    }
}

