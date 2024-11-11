using LIT.Smabu.Shared;

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
            if (from == DateTime.MinValue)
            {
                throw new ArgumentNullException(nameof(from));
            }
            else if (to != null && from > to)
            {
                return new(DateOnly.FromDateTime(to.Value), DateOnly.FromDateTime(to.Value));
            }
            else
            {
                return new(DateOnly.FromDateTime(from), to != null ? DateOnly.FromDateTime(to.Value) : null);
            }
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
            if (To == null)
            {
                return $"{From.Month:00}.{From.Year}-??.????";
            }
            else if (From.Month == To?.Month)
            {
                return $"{To?.Month:00}.{To?.Year}";
            }
            else
            {
                return $"{From.Month:00}.{From.Year}-{To?.Month:00}.{To?.Year}";
            }
        }
    }
}

