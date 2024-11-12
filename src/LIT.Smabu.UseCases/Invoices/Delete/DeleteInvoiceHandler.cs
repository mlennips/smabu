using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate.Services;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.Delete
{
    public class DeleteInvoiceHandler(DeleteInvoiceService deleteInvoiceService) : ICommandHandler<DeleteInvoiceCommand>
    {
        public async Task<Result> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = await deleteInvoiceService.DeleteAsync(request.InvoiceId);
            return result;
        }
    }
}
