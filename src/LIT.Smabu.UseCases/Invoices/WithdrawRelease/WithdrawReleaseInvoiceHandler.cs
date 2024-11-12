using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.WithdrawRelease
{
    public class WithdrawReleaseInvoiceHandler(IAggregateStore store) : ICommandHandler<WithdrawReleaseInvoiceCommand>
    {
        public async Task<Result> Handle(WithdrawReleaseInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await store.GetByAsync(request.InvoiceId);
            var result = invoice.WithdrawRelease();
            if (result.IsFailure)
            {
                return result.Error;
            }

            await store.UpdateAsync(invoice);
            return Result.Success();
        }
    }
}
