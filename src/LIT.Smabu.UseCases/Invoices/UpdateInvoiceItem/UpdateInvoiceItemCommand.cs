﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Shared.Interfaces;

namespace LIT.Smabu.UseCases.Invoices.UpdateInvoiceItem
{
    public record UpdateInvoiceItemCommand : ICommand<InvoiceItemDTO>
    {
        public required InvoiceItemId Id { get; set; }
        public required InvoiceId InvoiceId { get; set; }
        public required string Details { get; set; }
        public required Quantity Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}