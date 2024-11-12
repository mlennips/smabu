using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.AddOfferItem
{
    public record AddOfferItemCommand : ICommand<OfferItemId>
    {
        public required OfferItemId OfferItemId { get; set; }
        public required OfferId OfferId { get; set; }
        public required string Details { get; set; }
        public required Quantity Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public CatalogItemId? CatalogItemId { get; set; }
    }
}
