using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class UpdateInvoice
    {
        public record UpdateInvoiceCommand(InvoiceId InvoiceId, DatePeriod PerformancePeriod, TaxRate TaxRate, DateOnly? InvoiceDate) : ICommand<InvoiceId>;

        public class UpdateInvoiceHandler(IAggregateStore store) : ICommandHandler<UpdateInvoiceCommand, InvoiceId>
        {
            public async Task<Result<InvoiceId>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
            {
                var invoice = await store.GetByAsync(request.InvoiceId);
                var result = invoice.Update(request.PerformancePeriod, request.TaxRate, request.InvoiceDate);
                if (result.IsSuccess)
                {
                    await store.UpdateAsync(invoice);
                    return invoice.Id;
                }
                else
                {
                    return result.Error;
                }
            }
        }
    }
}