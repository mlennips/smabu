using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.OfferAggregate
{
    public record OfferNumber(long Value) : BusinessNumber(Value)
    {
        public override string ShortForm => "OFR";

        public override int Digits => 4;

        public static OfferNumber CreateLegacy(int id)
        {
            return new OfferNumber(id);
        }

        internal static OfferNumber CreateFirst()
        {
            return new OfferNumber(1);
        }

        internal static OfferNumber CreateNext(OfferNumber lastNumber)
        {
            return new OfferNumber(lastNumber.Value + 1);
        }
    }
}
