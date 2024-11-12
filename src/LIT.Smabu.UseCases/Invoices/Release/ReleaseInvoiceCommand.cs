using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.Release
{
    public record ReleaseInvoiceCommand : ICommand
    {
        public required InvoiceId InvoiceId { get; set; }
        public InvoiceNumber? Number { get; set; }
        public DateTime? ReleasedAt { get; set; }
    }
}
