using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class MoveInvoiceItemDown
    {
        public record MoveInvoiceItemDownCommand(InvoiceItemId InvoiceItemId, InvoiceId InvoiceId) : ICommand;

        public class MoveInvoiceItemDownHandler(IAggregateStore store) : ICommandHandler<MoveInvoiceItemDownCommand>
        {
            public async Task<Result> Handle(MoveInvoiceItemDownCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice = await store.GetByAsync(request.InvoiceId);
                Result result = invoice.MoveItemDown(request.InvoiceItemId);
                if (result.IsFailure)
                {
                    return result.Error;
                }

                await store.UpdateAsync(invoice);
                return Result.Success();
            }
        }
    }
}