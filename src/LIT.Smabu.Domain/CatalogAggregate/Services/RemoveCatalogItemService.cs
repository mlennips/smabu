using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using System.Reflection.Metadata.Ecma335;

namespace LIT.Smabu.Domain.CatalogAggregate.Services
{
    public class RemoveCatalogItemService(IAggregateRepository repository)
    {
        public async Task<Result> RemoveAsync(CatalogId catalogId, CatalogItemId catalogItemId)
        {
            var hasRelations = await CheckHasOffers(catalogItemId) || await CheckHasInvoices(catalogItemId);
            if (hasRelations)
            {
                return CommonErrors.HasReferences;
            }

            Catalog catalog = await repository.GetByAsync(catalogId);
            Result result = catalog.RemoveItem(catalogItemId);
            await repository.UpdateAsync(catalog);
            return result;
        }

        private async Task<bool> CheckHasOffers(CatalogItemId id)
        {
            IReadOnlyList<Offer> offers = await repository.GetAllAsync<Offer>();
            var isUsedInOffer = offers.Any(offer => offer.Items.Any(item => item.CatalogItemId == id));
            return isUsedInOffer;
        }

        private async Task<bool> CheckHasInvoices(CatalogItemId id)
        {
            IReadOnlyList<Invoice> invoices = await repository.GetAllAsync<Invoice>();
            var isUsedInInvoice = invoices.Any(offer => offer.Items.Any(item => item.CatalogItemId == id));
            return isUsedInInvoice;
        }
    }
}
