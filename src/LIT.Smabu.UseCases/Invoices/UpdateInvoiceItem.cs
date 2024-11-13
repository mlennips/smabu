using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class UpdateInvoiceItem
    {
        public record UpdateInvoiceItemCommand(InvoiceItemId InvoiceItemId, InvoiceId InvoiceId, string Details,
            Quantity Quantity, decimal UnitPrice, CatalogItemId? CatalogItemId) : ICommand<InvoiceItemId>;

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
}