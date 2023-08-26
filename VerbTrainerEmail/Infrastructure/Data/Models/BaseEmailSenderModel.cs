using System;
namespace VerbTrainerEmail.Infrastructure.Data.Models
{
    public abstract class BaseEmailSenderModel
	{
        public int Id { get; private set; }
		protected BaseEmailSenderModel()
		{
		}

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseEmailSenderModel;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (ReferenceEquals(null, compareTo))
                return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(BaseEmailSenderModel a, BaseEmailSenderModel b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(BaseEmailSenderModel? a, BaseEmailSenderModel? b) => !(a == b);

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }
    }
}
