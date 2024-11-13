using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Invoices;
using LIT.Smabu.UseCases.Offers;
using System.Globalization;

namespace LIT.Smabu.Infrastructure.Reports
{
    internal class QuestReportFactory(IAggregateStore store, ReportsConfig config) : IReportFactory
    {
        public async Task<IReport> CreateInvoiceReportAsync(IEntityId id)
        {
            var invoice = await store.GetByAsync((InvoiceId)id);
            var customer = await store.GetByAsync(invoice.CustomerId);
            var invoiceDTO = InvoiceDTO.Create(invoice, customer, true);
            var report = new InvoiceReport(invoiceDTO, config);
            return new QuestReport(report);
        }

        public async Task<IReport> CreateOfferReportAsync(IEntityId id)
        {
            var offer = await store.GetByAsync((OfferId)id);
            var customer = await store.GetByAsync(offer.CustomerId);
            var invoiceDTO = OfferDTO.Create(offer, customer, true);
            var report = new OfferReport(invoiceDTO, config);
            return new QuestReport(report);
        }
    }
}
