using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.UpdateInvoiceItem
{
    public class UpdateInvoiceItemHandler(IAggregateStore store) : ICommandHandler<UpdateInvoiceItemCommand, InvoiceItemId>
    {
        public async Task<Result<InvoiceItemId>> Handle(UpdateInvoiceItemCommand request, CancellationToken cancellationToken)
        {
            var invoice = await store.GetByAsync(request.InvoiceId);
            var invoiceItemResult = invoice.UpdateItem(request.InvoiceItemId, request.Details, request.Quantity, request.UnitPrice, request.CatalogItemId);
            if (invoiceItemResult.IsFailure)
            {
                return invoiceItemResult.Error;
            }
            await store.UpdateAsync(invoice);
            return invoiceItemResult.Value!.Id;
        }
    }
}
