using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class ReleaseInvoice
    {
        public record ReleaseInvoiceCommand(InvoiceId InvoiceId, InvoiceNumber? Number, DateTime? ReleasedAt) : ICommand;

        public class ReleaseInvoiceHandler(IAggregateRepository repository, BusinessNumberService businessNumberService) : ICommandHandler<ReleaseInvoiceCommand>
        {
            public async Task<Result> Handle(ReleaseInvoiceCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice = await repository.GetByAsync(request.InvoiceId);
                InvoiceNumber number = await businessNumberService.CreateInvoiceNumberAsync(invoice.Number, request.Number, invoice.FiscalYear);
                Result result = invoice.Release(number, request.ReleasedAt);
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
