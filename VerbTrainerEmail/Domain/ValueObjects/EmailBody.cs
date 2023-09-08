using System;
using VerbTrainerEmail.Domain.Base;

namespace VerbTrainerEmail.Domain.ValueObjects
{
    public class EmailBody : BaseValueObject
    {
        public string Text { get; private set; }

        private EmailBody(string text)
        {
            Text = text;
        }

        public static EmailBody Create(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Email body is null or empty.", nameof(text));
            }

            return new EmailBody(text);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Text;
        }
    }
}