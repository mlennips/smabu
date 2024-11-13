using LIT.Smabu.Core;
using System.Globalization;

namespace LIT.Smabu.Domain.Common
{
    public record Currency(string IsoCode, string Name, string Sign) : IValueObject
    {
        public string Format(decimal amount)
        {
            var numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            numberFormatInfo.CurrencySymbol = Sign;
            return amount.ToString("C", numberFormatInfo);
        }

        public static Currency EUR => new("EUR", "Euro", "€");
        public static Currency USD => new("USD", "US-Dollar", "$");

        public static Currency[] GetAll()
        {
            return [EUR, USD];
        }

        public override string ToString()
        {
            return $"{Name} ({Sign})";
        }

        public override int GetHashCode()
        {
            return IsoCode.GetHashCode();
        }
    }
}

