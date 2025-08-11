using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class RemoveInvoiceItem
    {
        public record RemoveInvoiceItemCommand(InvoiceItemId InvoiceItemId, InvoiceId InvoiceId) : ICommand;

        public class RemoveInvoiceItemHandler(IAggregateRepository repository) : ICommandHandler<RemoveInvoiceItemCommand>
        {
            public async Task<Result> Handle(RemoveInvoiceItemCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice = await repository.GetByAsync(request.InvoiceId);
                Result result = invoice.RemoveItem(request.InvoiceItemId);
                if (result.IsFailure)
                {
                    return result.Error;
                }

                await repository.UpdateAsync(invoice);
                return Result.Success();
            }
        }
    }
}