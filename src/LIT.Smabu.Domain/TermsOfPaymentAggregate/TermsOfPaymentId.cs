using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.TermsOfPaymentAggregate
{
    public record TermsOfPaymentId(Guid Value) : EntityId<TermsOfPayment>(Value);
}