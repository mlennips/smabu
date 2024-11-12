using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.UseCases.Customers;

namespace LIT.Smabu.UseCases.Offers
{
    public record OfferDTO : IDTO
    {
        public string DisplayName => Number.DisplayName + "/" + Customer.CorporateDesign.ShortName + "/" + OfferDate.ToShortDateString();
        public OfferId Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public CustomerDTO Customer { get; set; }
        public OfferNumber Number { get; set; }
        public DateOnly OfferDate { get; set; }
        public DateOnly ExpiresOn { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public TaxRate TaxRate { get; set; }
        public List<OfferItemDTO>? Items { get; set; }

        public OfferDTO(OfferId id, DateTime? createdAt, CustomerDTO customer, OfferNumber number, DateOnly offerDate,
                        DateOnly expiresOn, decimal amount, Currency currency, TaxRate taxRate)
        {
            Id = id;
            CreatedAt = createdAt;
            Customer = customer;
            Number = number;
            OfferDate = offerDate;
            ExpiresOn = expiresOn;
            Amount = amount;
            Currency = currency;
            TaxRate = taxRate;
        }

        public static OfferDTO Create(Offer offer, Customer customer, bool withItems = false)
        {
            var result = new OfferDTO(offer.Id, offer.Meta?.CreatedAt, CustomerDTO.Create(customer), offer.Number, offer.OfferDate, offer.ExpiresOn,
                offer.Amount, offer.Currency, offer.TaxRate);

            if (withItems)
            {
                result.Items = offer.Items.Select(OfferItemDTO.Create).ToList();
            }

            return result;
        }
    }
}