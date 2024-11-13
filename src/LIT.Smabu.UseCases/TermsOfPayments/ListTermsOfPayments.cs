using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.TermsOfPayments
{
    public static class ListTermsOfPayments
    {
        public record ListTermsOfPaymentsQuery : IQuery<TermsOfPaymentDTO[]>;

        public class GetTermsOfPaymentsHandler(IAggregateStore store) : IQueryHandler<ListTermsOfPaymentsQuery, TermsOfPaymentDTO[]>
        {
            public async Task<Result<TermsOfPaymentDTO[]>> Handle(ListTermsOfPaymentsQuery request, CancellationToken cancellationToken)
            {
                IReadOnlyList<TermsOfPayment> termsOfPayments = await store.GetAllAsync<TermsOfPayment>();
                return termsOfPayments.Select(TermsOfPaymentDTO.Create).OrderByDescending(x => x.Title).ToArray();
            }
        }
    }
}
