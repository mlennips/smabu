using LIT.Smabu.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public record PaymentMethod : SimpleValueObject<string>
    {
        private static readonly List<string> _values =
        [
            "Unknown",
            "Bank Transfer",
            "Direct Debit",
            "Credit Card",
            "Cash"
        ];

        public PaymentMethod(string value) : base(value)
        {
            if (!_values.Contains(value))
            {
                throw new ArgumentException($"Invalid payment method: {value}");
            }
        }

        public static PaymentMethod Default => BankTransfer;
        public static PaymentMethod Unknown => new("Unknown");
        public static PaymentMethod BankTransfer => new("Bank Transfer");
        public static PaymentMethod DirectDebit => new("Direct Debit");
        public static PaymentMethod CreditCard => new("Credit Card");
        public static PaymentMethod Cash => new("Cash");

        public static PaymentMethod[] GetAll()
        {
            return _values.Select(method => new PaymentMethod(method)).ToArray();
        }
    }
}
