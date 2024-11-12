using LIT.Smabu.Domain.Base;
using System.Globalization;

namespace LIT.Smabu.Domain.OrderAggregate
{
    public record OrderNumber(long Value) : BusinessNumber(Value)
    {
        public override string ShortForm => "ORD";

        public override int Digits => 8;

        public static OrderNumber CreateFirst(int year)
        {
            return new OrderNumber(long.Parse($"{year}0001", CultureInfo.InvariantCulture));
        }

        public static OrderNumber CreateNext(OrderNumber lastNumber)
        {
            return new OrderNumber(lastNumber.Value + 1);
        }
    }
}