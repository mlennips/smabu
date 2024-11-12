using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Payments
{
    public class PaymentDTO(PaymentId id, PaymentNumber number, PaymentDirection direction, DateTime accountingDate, string details,
            string payer, string payee, CustomerId? customerId, InvoiceId? invoiceId, string referenceNr, DateTime? referenceDate,
            decimal amountDue, DateTime? dueDate, bool isOverdue, decimal amountPaid, DateTime? paidAt, Currency currency, PaymentStatus status) : IDTO
    {
        public PaymentId Id { get; } = id;
        public PaymentNumber Number { get; } = number;
        public PaymentDirection Direction { get; } = direction;
        public DateTime AccountingDate { get; } = accountingDate;
        public string Details { get; } = details;
        public string Payer { get; } = payer;
        public string Payee { get; } = payee;
        public CustomerId? CustomerId { get; } = customerId;
        public InvoiceId? InvoiceId { get; } = invoiceId;
        public string ReferenceNr { get; } = referenceNr;
        public DateTime? ReferenceDate { get; } = referenceDate;
        public decimal AmountDue { get; } = amountDue;
        public DateTime? DueDate { get; } = dueDate;
        public bool IsOverdue { get; } = isOverdue;
        public decimal AmountPaid { get; } = amountPaid;
        public DateTime? PaidAt { get; } = paidAt;
        public Currency Currency { get; } = currency;
        public PaymentStatus Status { get; } = status;

        public string DisplayName
        {
            get
            {
                return $"{Direction.Value} {Number.DisplayName} / {Payer}{Payee} / {ReferenceNr}";
            }
        }

        internal static PaymentDTO Create(Payment payment)
        {
            return new PaymentDTO(payment.Id, payment.Number, payment.Direction, payment.AccountingDate, payment.Details, payment.Payer, payment.Payee,
                payment.CustomerId, payment.InvoiceId, payment.ReferenceNr, payment.ReferenceDate,
                payment.AmountDue, payment.DueDate, payment.CheckIsOverdue(), payment.AmountPaid, payment.PaidAt, payment.Currency, payment.Status);
        }
    }
}