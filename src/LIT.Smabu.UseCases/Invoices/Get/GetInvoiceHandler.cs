using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.Get
{
    public class GetInvoiceHandler(IAggregateStore store) : IQueryHandler<GetInvoiceQuery, InvoiceDTO>
    {
        public async Task<Result<InvoiceDTO>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoice = await store.GetByAsync(request.InvoiceId);
            var customer = await store.GetByAsync(invoice.CustomerId);
            var result = InvoiceDTO.Create(invoice, customer, request.WithItems);
            return result;
        }
    }
}
