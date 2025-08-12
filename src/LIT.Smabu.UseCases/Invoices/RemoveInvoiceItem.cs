using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class RemoveInvoiceItem
    {
        public record RemoveInvoiceItemCommand(InvoiceItemId InvoiceItemId, InvoiceId InvoiceId) : ICommand;

        public class RemoveInvoiceItemHandler(IUnitOfWork uow) : ICommandHandler<RemoveInvoiceItemCommand>
        {
            public async Task<Result> Handle(RemoveInvoiceItemCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice = await uow.Repository.GetByAsync(request.InvoiceId);
                Result result = invoice.RemoveItem(request.InvoiceItemId);
                if (result.IsFailure)
                {
                    return result.Error;
                }

                await uow.Repository.UpdateAsync(invoice);
                return Result.Success();
            }
        }
    }
}