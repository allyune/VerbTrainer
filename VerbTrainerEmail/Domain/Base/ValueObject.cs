using System;
namespace VerbTrainerEmail.Domain.Base
{
    public abstract class ValueObject
    {
        // Compare ValueObject by values of the properties 
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ValueObject other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
        }

        // Must be implemented by child class for equality comparison
        protected abstract IEnumerable<object> GetEqualityComponents();
    }

}

