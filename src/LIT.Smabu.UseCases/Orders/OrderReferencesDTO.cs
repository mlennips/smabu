using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.OrderAggregate;

namespace LIT.Smabu.UseCases.Orders
{
    public record OrderReferencesDTO(List<OrderReferenceDTO<OfferId>> Offers, List<OrderReferenceDTO<InvoiceId>> Invoices)
    {
        internal static OrderReferencesDTO Create(OrderReferences references, List<Offer> allOffers, List<Invoice> allInvoices)
        {
            var offerReferences = references.OfferIds.Select(oId => allOffers.Single(offer => offer.Id == oId))
                .Select(offer => new OrderReferenceDTO<OfferId>(offer.Id, offer.Number.ToString(), true, offer.OfferDate, offer.Amount))
                .ToList();

            var invoiceReferences = references.InvoiceIds.Select(iId => allInvoices.Single(invoice => invoice.Id == iId))
                .Select(invoice => new OrderReferenceDTO<InvoiceId>(invoice.Id, invoice.Number.ToString(), true, invoice.InvoiceDate, invoice.Amount))
                .ToList();

            return new OrderReferencesDTO(offerReferences, invoiceReferences);
        }
    }
}