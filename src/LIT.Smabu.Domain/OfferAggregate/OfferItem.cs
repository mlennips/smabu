﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.ProductAggregate;
using LIT.Smabu.Domain.Shared;

namespace LIT.Smabu.Domain.OfferAggregate
{
    public class OfferItem : Entity<OfferItemId>
    {
        public OfferItem(OfferItemId id, OfferId offerId, int position, string details, Quantity quantity, decimal unitPrice, ProductId? productId = null)
        {
            Id = id;
            OfferId = offerId;
            Position = position;
            Details = details;
            Quantity = quantity;
            UnitPrice = unitPrice;
            ProductId = productId;
            RefreshTotalPrice();
        }

        public override OfferItemId Id { get; }
        public OfferId OfferId { get; }
        public int Position { get; private set; }
        public string Details { get; private set; }
        public Quantity Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        public ProductId? ProductId { get; private set; }

        internal void Edit(string details, Quantity quantity, decimal unitPrice)
        {
            Details = details;
            Quantity = quantity;
            UnitPrice = unitPrice;
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

