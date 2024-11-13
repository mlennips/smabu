using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.Common
{
    public record Quantity : IValueObject
    {
        public decimal Value { get; }
        public Unit Unit { get; }

        public Quantity(decimal value, Unit unit)
        {
            Value = value;
            Unit = unit ?? throw new ArgumentException("Unit must be specified.");
        }

        public static Quantity Empty() => new(0, Unit.None);

        public override string ToString() => $"{Value} {Unit.Value}";
    }
}