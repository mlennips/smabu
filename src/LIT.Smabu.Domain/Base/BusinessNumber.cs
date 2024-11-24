using System.Globalization;

namespace LIT.Smabu.Domain.Base
{
    public abstract record BusinessNumber : SimpleValueObject<long>, IHasDisplayName
    {
        private const string TempText = "TEMP";

        public abstract string ShortForm { get; }
        public abstract int Digits { get; }
        protected virtual int TemporaryValue { get; }
        public bool IsTemporary => Value == TemporaryValue;
        public string DisplayName { get; }

        protected BusinessNumber(long value) : base(value)
        {
            DisplayName = $"{ShortForm}-{(IsTemporary ? TempText : ConvertValueToFormattedString())}";
        }

        public override int CompareTo(SimpleValueObject<long>? other)
        {
            return other is not null
            ? string.Compare(DisplayName, ((BusinessNumber)other).DisplayName, StringComparison.Ordinal) : -1;
        }

        private string ConvertValueToFormattedString()
        {
            return Value.ToString(new string('0', Digits), CultureInfo.InvariantCulture);
        }
    }
}
