using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class AddInvoiceItem
    {
        public record AddInvoiceItemCommand(InvoiceItemId InvoiceItemId, InvoiceId InvoiceId, string Details,
            Quantity Quantity, decimal UnitPrice, CatalogItemId? CatalogItemId) : ICommand<InvoiceItemId>;

        public class AddInvoiceItemHandler(IAggregateStore store) : ICommandHandler<AddInvoiceItemCommand, InvoiceItemId>
        {
            public async Task<Result<InvoiceItemId>> Handle(AddInvoiceItemCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice = await store.GetByAsync(request.InvoiceId);
                Result<InvoiceItem> invoiceLineResult = invoice.AddItem(request.InvoiceItemId, request.Details, request.Quantity, request.UnitPrice, request.CatalogItemId);
                if (invoiceLineResult.IsFailure)
                {
                    return invoiceLineResult.Error;
                }

                await store.UpdateAsync(invoice);
                return invoiceLineResult.Value!.Id;
            }
        }
    }
}