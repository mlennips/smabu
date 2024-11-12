using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Events;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.UseCases.Shared;
using MediatR;

namespace LIT.Smabu.UseCases.Payments.Create
{
    public record CreatePaymentCommand : ICommand<PaymentId>
    {
        public PaymentId PaymentId { get; init; }
        public PaymentDirection Direction { get; init; }
        public DateTime AccountingDate { get; init; }
        public string Details { get; init; }
        public string Payer { get; init; }
        public string Payee { get; init; }
        public CustomerId? CustomerId { get; init; }
        public InvoiceId? InvoiceId { get; init; }
        public string ReferenceNr { get; init; }
        public DateTime? ReferenceDate { get; init; }
        public decimal AmountDue { get; init; }
        public DateTime? DueDate { get; init; }

        public bool? MarkAsPaid { get; init; }

        public CreatePaymentCommand(PaymentId paymentId, PaymentDirection direction, DateTime accountingDate, string details,
            string payer, string payee, CustomerId? customerId, InvoiceId? invoiceId, string referenceNr, DateTime? referenceDate, decimal amountDue,
            bool? markAsPaid = false)
        {
            PaymentId = paymentId;
            Direction = direction;
            AccountingDate = accountingDate;
            Details = details;
            Payer = payer;
            Payee = payee;
            CustomerId = customerId;
            InvoiceId = invoiceId;
            ReferenceNr = referenceNr;
            ReferenceDate = referenceDate;
            AmountDue = amountDue;
            MarkAsPaid = markAsPaid;
        }

        internal static CreatePaymentCommand Create(Invoice invoice, Customer customer)
        {
            return new CreatePaymentCommand(
                new PaymentId(Guid.NewGuid()),
                PaymentDirection.Incoming,
                invoice.InvoiceDate!.Value.ToDateTime(TimeOnly.MinValue),
                "",
                customer.Name,
                "",
                customer.Id,
                invoice.Id,
                invoice.Number.DisplayName,
                invoice.InvoiceDate!.Value.ToDateTime(TimeOnly.MinValue),
                invoice.Amount,
                false);
        }

        internal bool Validate()
        {
            if (PaymentId == null || Direction == null)
            {
                return false;
            }

            if (Direction == PaymentDirection.Incoming
                && (CustomerId == null || InvoiceId == null))
            {
                return false;
            }

            if (Direction == PaymentDirection.Outgoing
                && (CustomerId != null || InvoiceId != null))
            {
                return false;
            }

            return true;
        }
    }
}
