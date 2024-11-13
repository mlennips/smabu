using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Payments
{
    public static class ListPayments
    {
        public record ListPaymentsQuery : IQuery<PaymentDTO[]>;

        public class ListPaymentsHandler(IAggregateStore store) : IQueryHandler<ListPaymentsQuery, PaymentDTO[]>
        {
            public async Task<Result<PaymentDTO[]>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
            {
                var payments = await store.GetAllAsync<Payment>();
                return payments.Select(p => PaymentDTO.Create(p)).ToArray();
            }
        }
    }
}