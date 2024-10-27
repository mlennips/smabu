﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.UseCases.Customers;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices
{
    public record InvoiceDTO : IDTO
    {
        public InvoiceDTO(InvoiceId id, DateTime? createdAt, CustomerDTO customer, InvoiceNumber number, decimal amount,
                          Currency currency, DatePeriod performancePeriod, int fiscalYear, TaxRate taxRate, bool isReleased, DateTime? releasedAt)
        {
            Id = id;
            CreatedAt = createdAt;
            Customer = customer;
            Number = number;
            Amount = amount;
            Currency = currency;
            PerformancePeriod = performancePeriod;
            FiscalYear = fiscalYear;
            TaxRate = taxRate;
            IsReleased = isReleased;
            ReleasedAt = releasedAt;
        }

        public string DisplayName => $"{Number?.Long} {Customer?.ShortName}";
        public InvoiceId Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public CustomerDTO Customer { get; set; }
        public InvoiceNumber Number { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public virtual DatePeriod PerformancePeriod { get; set; }
        public int FiscalYear { get; set; }
        public TaxRate TaxRate { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public List<InvoiceItemDTO>? Items { get; set; }

        public static InvoiceDTO Create(Invoice invoice, Customer customer, bool withItems = false)
        {
            var result = new InvoiceDTO(invoice.Id, invoice.Meta?.CreatedAt, CustomerDTO.Create(customer), invoice.Number,
                invoice.Amount, invoice.Currency, invoice.PerformancePeriod, invoice.FiscalYear, invoice.TaxRate, invoice.IsReleased, invoice.ReleasedAt);

            if (withItems)
            {
                result.Items = invoice.Items.Select(InvoiceItemDTO.Create).ToList();
            }

            return result;
        }
    }
}