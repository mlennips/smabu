using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class GetInvoice
    {
        public record GetInvoiceQuery(InvoiceId InvoiceId, bool WithItems = false) : IQuery<InvoiceDTO>;

        public class GetInvoiceHandler(IAggregateStore store) : IQueryHandler<GetInvoiceQuery, InvoiceDTO>
        {
            public async Task<Result<InvoiceDTO>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
            {
                Invoice invoice = await store.GetByAsync(request.InvoiceId);
                Domain.CustomerAggregate.Customer customer = await store.GetByAsync(invoice.CustomerId);
                var result = InvoiceDTO.Create(invoice, customer, request.WithItems);
                return result;
            }
        }
    }
}