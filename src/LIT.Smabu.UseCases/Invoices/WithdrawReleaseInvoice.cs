using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class WithdrawReleaseInvoice
    {
        public record WithdrawReleaseInvoiceCommand(InvoiceId InvoiceId) : ICommand;

        public class WithdrawReleaseInvoiceHandler(IAggregateRepository repository) : ICommandHandler<WithdrawReleaseInvoiceCommand>
        {
            public async Task<Result> Handle(WithdrawReleaseInvoiceCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice = await repository.GetByAsync(request.InvoiceId);
                Result result = invoice.WithdrawRelease();
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