using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.AddInvoiceItem
{
    public class AddInvoiceItemHandler(IAggregateStore store) : ICommandHandler<AddInvoiceItemCommand, InvoiceItemId>
    {
        public async Task<Result<InvoiceItemId>> Handle(AddInvoiceItemCommand request, CancellationToken cancellationToken)
        {
            var invoice = await store.GetByAsync(request.InvoiceId);
            var invoiceLineResult = invoice.AddItem(request.InvoiceItemId, request.Details, request.Quantity, request.UnitPrice, request.CatalogItemId);
            if (invoiceLineResult.IsFailure)
            {
                return invoiceLineResult.Error;
            }

            await store.UpdateAsync(invoice);
            return invoiceLineResult.Value!.Id;
        }
    }
}
