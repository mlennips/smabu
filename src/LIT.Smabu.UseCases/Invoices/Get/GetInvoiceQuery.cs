using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.Get
{
    public record GetInvoiceQuery : IQuery<InvoiceDTO>
    {
        public InvoiceId InvoiceId { get; }
        public bool WithItems { get; set; }

        public GetInvoiceQuery(InvoiceId invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}