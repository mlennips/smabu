using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace LIT.Smabu.Domain.CatalogAggregate.Services
{
    public class RemoveCatalogItemService(IUnitOfWork uow)
    {
        public async Task<Result> RemoveAsync(CatalogId catalogId, CatalogItemId catalogItemId)
        {
            var hasRelations = await CheckHasOffers(catalogItemId) || await CheckHasInvoices(catalogItemId);
            if (hasRelations)
            {
                return CommonErrors.HasReferences;
            }

            Catalog catalog = await uow.Repository.GetByAsync(catalogId);
            Result result = catalog.RemoveItem(catalogItemId);
            await uow.Repository.UpdateAsync(catalog);
            return result;
        }

        private async Task<bool> CheckHasOffers(CatalogItemId id)
        {
            IReadOnlyList<Offer> offers = await uow.Repository.GetAllAsync<Offer>();
            var isUsedInOffer = offers.Any(offer => offer.Items.Any(item => item.CatalogItemId == id));
            return isUsedInOffer;
        }

        private async Task<bool> CheckHasInvoices(CatalogItemId id)
        {
            IReadOnlyList<Invoice> invoices = await uow.Repository.GetAllAsync<Invoice>();
            var isUsedInInvoice = invoices.Any(offer => offer.Items.Any(item => item.CatalogItemId == id));
            return isUsedInInvoice;
        }
    }
}
