using LIT.Smabu.Shared;

namespace LIT.Smabu.Domain.Shared
{
    public abstract record SimpleValueObject<T> : IValueObject, IComparable<SimpleValueObject<T>>
    {
        public T Value { get; init; }

        public SimpleValueObject(T value)
        {
            if (value is null)
            {
                throw new ArgumentException("Wert darf nicht NULL sein.");
            }
            Value = value;
        }

        public override int GetHashCode() => Value!.GetHashCode();
        public virtual int CompareTo(SimpleValueObject<T>? other) => other is not null ? ToString().CompareTo(other.ToString()) : -1;
        public override string ToString() => Value?.ToString() ?? "";
    }
}