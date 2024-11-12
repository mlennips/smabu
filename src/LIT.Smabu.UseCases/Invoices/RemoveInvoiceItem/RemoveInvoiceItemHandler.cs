using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.RemoveInvoiceItem
{
    public class RemoveInvoiceItemHandler(IAggregateStore store) : ICommandHandler<RemoveInvoiceItemCommand>
    {
        public async Task<Result> Handle(RemoveInvoiceItemCommand request, CancellationToken cancellationToken)
        {
            var invoice = await store.GetByAsync(request.InvoiceId);
            var result = invoice.RemoveItem(request.InvoiceItemId);
            if (result.IsFailure)
            {
                return result.Error;
            }

            await store.UpdateAsync(invoice);
            return Result.Success();
        }
    }
}
