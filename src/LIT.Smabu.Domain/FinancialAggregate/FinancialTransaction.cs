using LIT.Smabu.Core;
using LIT.Smabu.Domain.PaymentAggregate;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public record FinancialTransaction : IValueObject
    {
        public DateTime Date { get; init; }
        public decimal Amount { get; init; }
        public string Description { get; init; }
        public FinancialCategory Category { get; init; }
        public PaymentId? PaymentId { get; init; }
        public bool IsImported => PaymentId != null;

        public FinancialTransaction(DateTime date, decimal amount, string description, FinancialCategory category, PaymentId? paymentId = null)
        {
            Date = date;
            Amount = amount;
            Description = description;
            Category = category;
            PaymentId = paymentId;
        }

        public static FinancialTransaction Create(DateTime date, decimal amount, string description, FinancialCategory category)
        {
            return new FinancialTransaction(date, amount, description, category);
        }

        public static FinancialTransaction CreateForPayment(Payment payment)
        {
            var description = $"{payment.ReferenceNr} {payment.Details}".Trim();
            return new FinancialTransaction(payment.AccountingDate!.Value, payment.AmountPaid, description, FinancialCategory.Revenue, payment.Id);
        }
    }
}