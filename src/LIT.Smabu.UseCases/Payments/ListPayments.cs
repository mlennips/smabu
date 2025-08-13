using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Payments
{
    public static class ListPayments
    {
        public record ListPaymentsQuery : IQuery<PaymentDTO[]>;

        public class ListPaymentsHandler(IAggregateCache cache) : IQueryHandler<ListPaymentsQuery, PaymentDTO[]>
        {
            public async Task<Result<PaymentDTO[]>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
            {
                IReadOnlyList<Payment> payments = await cache.GetAllAsync<Payment>();
                return payments.Select(PaymentDTO.Create).ToArray();
            }
        }
    }
}