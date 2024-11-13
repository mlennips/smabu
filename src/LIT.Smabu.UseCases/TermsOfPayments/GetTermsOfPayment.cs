using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.TermsOfPayments
{
    public static class GetTermsOfPayment
    {
        public record GetTermsOfPaymentQuery(TermsOfPaymentId TermsOfPaymentId) : IQuery<TermsOfPaymentDTO>;

        public class GetTermsOfPaymentHandler(IAggregateStore store) : IQueryHandler<GetTermsOfPaymentQuery, TermsOfPaymentDTO>
        {
            public async Task<Result<TermsOfPaymentDTO>> Handle(GetTermsOfPaymentQuery request, CancellationToken cancellationToken)
            {
                TermsOfPayment termsOfPayment = await store.GetByAsync(request.TermsOfPaymentId);
                return TermsOfPaymentDTO.Create(termsOfPayment);
            }
        }
    }
}