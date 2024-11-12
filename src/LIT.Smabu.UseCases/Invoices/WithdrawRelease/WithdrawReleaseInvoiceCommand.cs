using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.WithdrawRelease
{
    public record WithdrawReleaseInvoiceCommand : ICommand
    {
        public required InvoiceId InvoiceId { get; set; }
    }
}
