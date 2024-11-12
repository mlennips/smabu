using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.TermsOfPaymentAggregate
{
    public class TermsOfPayment(TermsOfPaymentId id, string title, string details, int? dueDays) : AggregateRoot<TermsOfPaymentId>
    {
        public override TermsOfPaymentId Id { get; } = id;
        public string Title { get; private set; } = title;
        public string Details { get; private set; } = details;
        public int? DueDays { get; private set; } = dueDays;

        public static TermsOfPayment Create(TermsOfPaymentId id, string title, string details, int? dueDays)
        {
            return new(id, title, details, dueDays);
        }
    }
}
