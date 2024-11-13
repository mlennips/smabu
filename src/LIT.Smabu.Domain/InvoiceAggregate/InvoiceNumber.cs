using LIT.Smabu.Domain.Base;
using System.Globalization;

namespace LIT.Smabu.Domain.InvoiceAggregate
{
    public record InvoiceNumber(long Value) : BusinessNumber(Value)
    {
        public override string ShortForm => "INV";

        public override int Digits => 8;

        public static InvoiceNumber CreateFirst(int year)
        {
            return new InvoiceNumber(long.Parse($"{year}0001", CultureInfo.InvariantCulture));
        }

        public static InvoiceNumber CreateNext(InvoiceNumber lastNumber)
        {
            return new InvoiceNumber(lastNumber.Value + 1);
        }

        public static InvoiceNumber CreateTmp()
        {
            return new InvoiceNumber(0);
        }

        public static InvoiceNumber CreateLegacy(long number)
        {
            return new InvoiceNumber(number);
        }
    }
}