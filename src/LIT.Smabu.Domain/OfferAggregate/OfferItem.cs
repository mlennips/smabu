using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;

namespace LIT.Smabu.Domain.OfferAggregate
{
    public class OfferItem : Entity<OfferItemId>
    {
        public override OfferItemId Id { get; }
        public OfferId OfferId { get; }
        public int Position { get; private set; }
        public string Details { get; private set; }
        public Quantity Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        public CatalogItemId? CatalogItemId { get; private set; }

        public OfferItem(OfferItemId id, OfferId offerId, int position, string details, Quantity quantity, decimal unitPrice, CatalogItemId? catalogItemId = null)
        {
            Id = id;
            OfferId = offerId;
            Position = position;
            Details = details;
            Quantity = quantity;
            UnitPrice = unitPrice;
            CatalogItemId = catalogItemId;
            RefreshTotalPrice();
        }

        internal void Edit(string details, Quantity quantity, decimal unitPrice, CatalogItemId? catalogItemId)
        {
            Details = details;
            Quantity = quantity;
            UnitPrice = unitPrice;
            CatalogItemId = catalogItemId;
            RefreshTotalPrice();
        }

        internal void EditPosition(int position)
        {
            Position = position;
        }

        private void RefreshTotalPrice()
        {
            TotalPrice = Quantity.Value * UnitPrice;
        }
    }
}

