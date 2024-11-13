using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.Common
{
    public record DatePeriod : IValueObject
    {
        public DateOnly From { get; }
        public DateOnly? To { get; }

        public DatePeriod(DateOnly from, DateOnly? to)
        {
            if (from == DateOnly.MinValue && to == DateOnly.MinValue)
            {
                throw new ArgumentException("Von und bis ungültig.");
            }
            if (to != null && from > to)
            {
                throw new ArgumentException("Von ist größer als bis.");
            }
            From = from;
            To = to;
        }

        public static DatePeriod Create(DateTime from, DateTime? to)
        {
            to = to == DateTime.MinValue ? null : to;
            return from == DateTime.MinValue
                ? throw new ArgumentNullException(nameof(from))
                : to != null && from > to
                    ? new(DateOnly.FromDateTime(to.Value), DateOnly.FromDateTime(to.Value))
                    : new(DateOnly.FromDateTime(from), to != null ? DateOnly.FromDateTime(to.Value) : null);
        }

        public override string ToString()
        {
            var result = From.ToShortDateString();
            if (To != null)
            {
                result += "-" + To.Value.ToShortDateString();
            }
            else
            {
                result += "-???";
            }
            return result;
        }

        public string ToStringInMonths()
        {
            return To == null
                ? $"{From.Month:00}.{From.Year}-??.????"
                : From.Month == To?.Month ? $"{To?.Month:00}.{To?.Year}" : $"{From.Month:00}.{From.Year}-{To?.Month:00}.{To?.Year}";
        }
    }
}

