using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.Create
{
    public class CreateInvoiceHandler(IAggregateStore store) : ICommandHandler<CreateInvoiceCommand, InvoiceId>
    {
        public async Task<Result<InvoiceId>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            Invoice invoice;
            var customer = await store.GetByAsync(request.CustomerId);
            request.PerformancePeriod ??= new DatePeriod(DateOnly.FromDateTime(DateTime.Now), null);

            if (request.TemplateId != null)
            {
                var template = await store.GetByAsync(request.TemplateId);
                invoice = Invoice.CreateFromTemplate(request.InvoiceId, request.CustomerId, request.FiscalYear, customer.MainAddress, request.PerformancePeriod, template);
            }
            else
            {
                invoice = Invoice.Create(request.InvoiceId, request.CustomerId, request.FiscalYear, customer.MainAddress, request.PerformancePeriod,
                    request.Currency, request.TaxRate ?? TaxRate.Default);
            }

            await store.CreateAsync(invoice);
            return invoice.Id;
        }
    }
}
