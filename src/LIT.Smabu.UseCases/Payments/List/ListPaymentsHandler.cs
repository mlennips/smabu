using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Payments.List
{
    public class ListPaymentsHandler(IAggregateStore store) : IQueryHandler<ListPaymentsQuery, PaymentDTO[]>
    {
        public async Task<Result<PaymentDTO[]>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await store.GetAllAsync<Payment>();
            return payments.Select(p => PaymentDTO.Create(p)).ToArray();
        }
    }
}
