using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Base;
using System.Globalization;

namespace LIT.Smabu.UseCases.Offers
{
    public record OfferItemDTO(OfferItemId Id, OfferId OfferId, int Position, string Details, Quantity Quantity, decimal UnitPrice, decimal TotalPrice, CatalogItemId? CatalogItemId) : IDTO
    {
        public string DisplayName => Position.ToString("00", CultureInfo.InvariantCulture);

        public static OfferItemDTO Create(OfferItem offerItem)
        {
            return new(offerItem.Id, offerItem.OfferId, offerItem.Position, offerItem.Details,
                offerItem.Quantity, offerItem.UnitPrice, offerItem.TotalPrice, offerItem.CatalogItemId);
        }
    }
}