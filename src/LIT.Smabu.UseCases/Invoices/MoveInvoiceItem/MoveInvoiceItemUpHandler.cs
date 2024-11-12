using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.MoveInvoiceItem
{
    public class MoveInvoiceItemUpHandler(IAggregateStore store) : ICommandHandler<MoveInvoiceItemUpCommand>
    {
        public async Task<Result> Handle(MoveInvoiceItemUpCommand request, CancellationToken cancellationToken)
        {
            var invoice = await store.GetByAsync(request.InvoiceId);
            var result = invoice.MoveItemUp(request.InvoiceItemId);
            if (result.IsFailure)
            {
                return result.Error;
            }

            await store.UpdateAsync(invoice);
            return Result.Success();
        }
    }
}
