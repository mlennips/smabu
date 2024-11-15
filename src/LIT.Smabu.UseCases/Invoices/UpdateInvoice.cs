using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.PaymentAggregate;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class UpdateInvoice
    {
        public record UpdateInvoiceCommand(InvoiceId InvoiceId, DatePeriod PerformancePeriod, TaxRate TaxRate, DateOnly? InvoiceDate, PaymentCondition PaymentCondition) : ICommand<InvoiceId>;

        public class UpdateInvoiceHandler(IAggregateStore store) : ICommandHandler<UpdateInvoiceCommand, InvoiceId>
        {
            public async Task<Result<InvoiceId>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice = await store.GetByAsync(request.InvoiceId);
                Result result = invoice.Update(request.PerformancePeriod, request.TaxRate, request.InvoiceDate, request.PaymentCondition);
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