using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class CreateInvoice
    {
        public record CreateInvoiceCommand(InvoiceId InvoiceId, CustomerId CustomerId, int FiscalYear,
            Currency Currency, DatePeriod? PerformancePeriod, TaxRate? TaxRate, InvoiceId? TemplateId) : ICommand<InvoiceId>;

        public class CreateInvoiceHandler(IAggregateStore store) : ICommandHandler<CreateInvoiceCommand, InvoiceId>
        {
            public async Task<Result<InvoiceId>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
            {
                Invoice invoice;
                Customer customer = await store.GetByAsync(request.CustomerId);
                DatePeriod performancePeriod = request.PerformancePeriod ?? new DatePeriod(DateOnly.FromDateTime(DateTime.Now), null);

                if (request.TemplateId != null)
                {
                    Invoice template = await store.GetByAsync(request.TemplateId);
                    invoice = Invoice.CreateFromTemplate(request.InvoiceId, request.CustomerId, request.FiscalYear, customer.MainAddress, performancePeriod, template);
                }
                else
                {
                    invoice = Invoice.Create(request.InvoiceId, request.CustomerId, request.FiscalYear, customer.MainAddress, performancePeriod,
                        request.Currency, request.TaxRate ?? TaxRate.Default);
                }

                await store.CreateAsync(invoice);
                return invoice.Id;
            }
        }
    }
}