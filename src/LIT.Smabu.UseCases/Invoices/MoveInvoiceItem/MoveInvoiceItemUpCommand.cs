using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.MoveInvoiceItem
{
    public record MoveInvoiceItemUpCommand(InvoiceItemId InvoiceItemId, InvoiceId InvoiceId) : ICommand
    {
    }
}
