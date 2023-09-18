using System;
namespace VerbTrainer.Infrastructure.Data.Models
{
    public abstract class BaseVerbTrainerModel
    {
        public int Id { get; set; }
        protected BaseVerbTrainerModel()
        {
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseVerbTrainerModel;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (ReferenceEquals(null, compareTo))
                return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(BaseVerbTrainerModel a, BaseVerbTrainerModel b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(BaseVerbTrainerModel? a, BaseVerbTrainerModel? b) => !(a == b);

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

