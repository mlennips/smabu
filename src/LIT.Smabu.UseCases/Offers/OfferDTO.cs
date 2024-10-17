﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Customers;
using LIT.Smabu.UseCases.Invoices;
using LIT.Smabu.UseCases.SeedWork;

namespace LIT.Smabu.UseCases.Offers
{
    public record OfferDTO : IDTO
    {
        public OfferDTO(OfferId id, DateTime? createdOn, CustomerDTO customer, OfferNumber number, DateOnly offerDate,
                        DateOnly expiresOn, decimal amount, Currency currency, TaxRate taxRate)
        {
            Id = id;
            CreatedOn = createdOn;
            Customer = customer;
            Number = number;
            OfferDate = offerDate;
            ExpiresOn = expiresOn;
            Amount = amount;
            Currency = currency;
            TaxRate = taxRate;
        }

        public string DisplayName => Number.Long + "/" + Customer.ShortName + "/" + CreatedOn?.ToShortDateString();
        public OfferId Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public CustomerDTO Customer { get; set; }
        public OfferNumber Number { get; set; }
        public DateOnly OfferDate { get; set; }
        public DateOnly ExpiresOn { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public TaxRate TaxRate { get; set; }

        public List<OfferItemDTO>? Items { get; set; }

        public static OfferDTO Create(Offer offer, Customer customer, bool withItems = false)
        {
            var result = new OfferDTO(offer.Id, offer.Meta?.CreatedOn, CustomerDTO.Create(customer), offer.Number, offer.OfferDate, offer.ExpiresOn,
                offer.Amount, offer.Currency, offer.TaxRate);

            if (withItems)
            {
                result.Items = offer.Items.Select(OfferItemDTO.Create).ToList();
            }

            return result;
        }
    }
}