using System;
using VerbTrainerEmail.Domain.Base;

namespace VerbTrainerEmail.Domain.ValueObjects
{
    public class EmailSubject : ValueObject
    {
        public string Value { get; }

        private EmailSubject(string value)
        {
            Value = value;
        }

        public static EmailSubject Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Subject value is null or empty.", nameof(value));
            }

            if (value.Length > 100)
            {
                throw new ArgumentException("Subject is longer than 100 charaters.", nameof(value));
            }

            return new EmailSubject(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}