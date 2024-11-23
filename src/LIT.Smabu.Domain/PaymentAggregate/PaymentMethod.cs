using LIT.Smabu.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public record PaymentMethod : EnumValueObject
    {
        private static readonly List<string> _values =
        [
            "Unknown",
            "BankTransfer",
            "DirectDebit",
            "CreditCard",
            "Cash"
        ];

        public override string Name => Value switch
        {
            "Unknown" => "Unbekannt",
            "BankTransfer" => "Überweisung",
            "DirectDebit" => "Lastschrift",
            "CreditCard" => "Kreditkarte",
            "Cash" => "Bar",
            _ => "?"
        };

        public PaymentMethod(string value) : base(value)
        {
            if (!_values.Contains(value))
            {
                throw new ArgumentException($"Invalid payment method: {value}");
            }
        }

        public static PaymentMethod Default => BankTransfer;
        public static PaymentMethod Unknown => new("Unknown");
        public static PaymentMethod BankTransfer => new("BankTransfer");
        public static PaymentMethod DirectDebit => new("DirectDebit");
        public static PaymentMethod CreditCard => new("CreditCard");
        public static PaymentMethod Cash => new("Cash");

        public static PaymentMethod[] GetAll()
        {
            return _values.Select(method => new PaymentMethod(method)).ToArray();
        }
    }
}
