using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using System.Globalization;

namespace LIT.Smabu.Domain.Common
{
    public record Currency(string Value) : EnumValueObject(Value)
    {
        public override string Name => Value switch
        {
            "EUR" => "Euro",
            "USD" => "US-Dollar",
            _ => "?"
        };

        public string IsoCode => Value;

        public string Sign => Value switch
        {
            "EUR" => "€",
            "USD" => "$",
            _ => "?"
        };

        public static Currency EUR => new("EUR");
        public static Currency USD => new("USD");

        public string Format(decimal amount)
        {
            var numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            numberFormatInfo.CurrencySymbol = Sign;
            return amount.ToString("C", numberFormatInfo);
        }

        public static Currency[] GetAll()
        {
            return [EUR, USD];
        }

        public override int GetHashCode()
        {
            return IsoCode.GetHashCode();
        }
    }
}

