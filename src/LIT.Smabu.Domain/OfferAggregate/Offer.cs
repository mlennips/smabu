using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using System.Xml.Linq;

namespace LIT.Smabu.Domain.OfferAggregate
{
    public class Offer(OfferId id, CustomerId customerId, OfferNumber number, Address customerAddress,
        DateOnly offerDate, DateOnly expiresOn, Currency currency, TaxRate taxRate, List<OfferItem> items)
        : AggregateRoot<OfferId>, IHasBusinessNumber<OfferNumber>
    {
        public override OfferId Id { get; } = id;
        public override string DisplayName => Number.DisplayName;
        public CustomerId CustomerId { get; } = customerId;
        public OfferNumber Number { get; } = number;
        public Address CustomerAddress { get; private set; } = customerAddress;
        public DateOnly OfferDate { get; private set; } = offerDate;
        public DateOnly ExpiresOn { get; private set; } = expiresOn;
        public Currency Currency { get; } = currency;
        public TaxRate TaxRate { get; private set; } = taxRate;
        public List<OfferItem> Items { get; } = items;
        public decimal Amount => Items.Sum(x => x.TotalPrice);

        public static Offer Create(OfferId id, CustomerId customerId, OfferNumber number, Address customerAddress,
            Currency currency, TaxRate taxRate)
        {
            return new Offer(id, customerId, number, customerAddress,
                DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
                currency, taxRate, []);
        }

        public Result Update(TaxRate taxRate, DateOnly offerDate, DateOnly expiresOn)
        {
            TaxRate = taxRate;
            OfferDate = offerDate;
            ExpiresOn = expiresOn;
            return Result.Success();
        }

        public Result<OfferItem> AddItem(OfferItemId id, string details, Quantity quantity, decimal unitPrice, CatalogItemId? catalogItemId = null)
        {
            var position = Items.OrderByDescending(x => x.Position).FirstOrDefault()?.Position + 1 ?? 1;
            var result = new OfferItem(id, Id, position, details, quantity, unitPrice, catalogItemId);
            Items.Add(result);
            return result;
        }

        public Result<OfferItem> UpdateItem(OfferItemId id, string details, Quantity quantity, decimal unitPrice, CatalogItemId? catalogItemId = null)
        {
            OfferItem item = Items.Find(x => x.Id == id)!;
            item.Edit(details, quantity, unitPrice, catalogItemId);
            return item;
        }

        public Result RemoveItem(OfferItemId id)
        {
            OfferItem item = Items.Find(x => x.Id == id)!;
            Items.Remove(item);
            ReorderItems();
            return Result.Success();
        }

        private void ReorderItems()
        {
            var pos = 1;
            foreach (OfferItem? item in Items.OrderBy(x => x.Position))
            {
                item.EditPosition(pos++);
            }
        }
    }
}

