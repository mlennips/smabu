using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Services;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class DeleteInvoice
    {
        public record DeleteInvoiceCommand(InvoiceId InvoiceId) : ICommand;
        public class DeleteInvoiceHandler(DeleteInvoiceService deleteInvoiceService) : ICommandHandler<DeleteInvoiceCommand>
        {
            public async Task<Result> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
            {
                var result = await deleteInvoiceService.DeleteAsync(request.InvoiceId);
                return result;
            }
        }
    }
}