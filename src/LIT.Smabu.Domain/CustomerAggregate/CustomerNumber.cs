using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.CustomerAggregate
{
    public record CustomerNumber(long Value) : BusinessNumber(Value)
    {
        public override string ShortForm => "CUS";

        public override int Digits => 4;

        public static CustomerNumber CreateFirst()
        {
            return new CustomerNumber(1);
        }

        public static CustomerNumber CreateNext(CustomerNumber lastNumber)
        {
            return new CustomerNumber(lastNumber.Value + 1);
        }

        public static CustomerNumber CreateLegacy(int id)
        {
            return new CustomerNumber(id);
        }
    }
}