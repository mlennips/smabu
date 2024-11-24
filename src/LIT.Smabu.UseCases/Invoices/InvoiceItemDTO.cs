using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.UseCases.Base;
using System.Globalization;

namespace LIT.Smabu.UseCases.Invoices
{
    public record InvoiceItemDTO : IDTO
    {
        public InvoiceItemId Id { get; set; }
        public InvoiceId InvoiceId { get; set; }
        public string DisplayName { get; }
        public int Position { get; set; }
        public string Details { get; set; }
        public Quantity Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public CatalogItemId? CatalogItemId { get; set; }

        public InvoiceItemDTO(InvoiceItemId id, InvoiceId invoiceId, string displayName, int position, string details, Quantity quantity,
                              decimal unitPrice, decimal totalPrice, CatalogItemId? catalogItemId)
        {
            Id = id;
            InvoiceId = invoiceId;
            DisplayName = displayName;
            Position = position;
            Details = details;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            CatalogItemId = catalogItemId;
        }

        public static InvoiceItemDTO Create(InvoiceItem invoiceItem)
        {
            return new(invoiceItem.Id, invoiceItem.InvoiceId, invoiceItem.DisplayName, invoiceItem.Position, invoiceItem.Details,
                invoiceItem.Quantity, invoiceItem.UnitPrice, invoiceItem.TotalPrice, invoiceItem.CatalogItemId);
        }
    }
}