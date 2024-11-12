using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.TermsOfPayments.Get
{
    public record GetTermsOfPaymentQuery : IQuery<TermsOfPaymentDTO>
    {
        public GetTermsOfPaymentQuery(TermsOfPaymentId termsOfPaymentId)
        {
            TermsOfPaymentId = termsOfPaymentId;
        }

        public TermsOfPaymentId TermsOfPaymentId { get; }
    }
}
