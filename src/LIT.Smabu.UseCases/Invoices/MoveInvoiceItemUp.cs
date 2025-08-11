using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class MoveInvoiceItemUp
    {
        public record MoveInvoiceItemUpCommand(InvoiceItemId InvoiceItemId, InvoiceId InvoiceId) : ICommand;

        public class MoveInvoiceItemUpHandler(IAggregateRepository repository) : ICommandHandler<MoveInvoiceItemUpCommand>
        {
            public async Task<Result> Handle(MoveInvoiceItemUpCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice = await repository.GetByAsync(request.InvoiceId);
                Result result = invoice.MoveItemUp(request.InvoiceItemId);
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