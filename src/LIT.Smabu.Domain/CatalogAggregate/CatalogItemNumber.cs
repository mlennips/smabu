using LIT.Smabu.Domain.Base;
using System.Globalization;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public record class CatalogItemNumber(long Value) : BusinessNumber(Value)
    {
        public override string ShortForm { get; } = "CAT";

        public override int Digits { get; } = 4;

        public static CatalogItemNumber CreateFirst()
        {
            return new CatalogItemNumber(1);
        }

        public static CatalogItemNumber CreateNext(CatalogItemNumber lastNumber)
        {
            return new CatalogItemNumber(lastNumber.Value + 1);
        }
    }
}