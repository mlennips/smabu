using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.TermsOfPayments.List
{
    public class GetTermsOfPaymentsHandler(IAggregateStore store) : IQueryHandler<ListTermsOfPaymentsQuery, TermsOfPaymentDTO[]>
    {
        public async Task<Result<TermsOfPaymentDTO[]>> Handle(ListTermsOfPaymentsQuery request, CancellationToken cancellationToken)
        {
            var termsOfPayments = await store.GetAllAsync<TermsOfPayment>();
            return termsOfPayments.Select(x => TermsOfPaymentDTO.Create(x)).OrderByDescending(x => x.Title).ToArray();
        }
    }
}
