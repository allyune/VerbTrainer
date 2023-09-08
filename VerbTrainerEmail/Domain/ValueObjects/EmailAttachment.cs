using System;
using VerbTrainerEmail.Domain.Base;
using static System.Net.Mime.MediaTypeNames;

namespace VerbTrainerEmail.Domain.ValueObjects
{
    public class EmailAttachment : BaseValueObject
    {
        public string FileName { get; private set; }
        public string MimeType { get; private set; }
        public byte[] FileData { get; private set; }

        private EmailAttachment(string fileName, string mimeTyte, byte[] fileData)
        {
            FileName = fileName;
            MimeType = mimeTyte;
            FileData = fileData;
        }

        public static EmailAttachment Create(string fileName, string mimeTyte, byte[] fileData)
        {
            // TODO: Implement validation
            return new EmailAttachment(fileName, mimeTyte, fileData);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FileName;
            yield return MimeType;
            yield return FileData;
        }
    }
}

