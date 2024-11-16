using LIT.Smabu.Core;
using LIT.Smabu.Domain.PaymentAggregate;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public record Transaction : IValueObject
    {
        public Guid Id { get; init; }
        public DateTime Date { get; init; }
        public decimal Amount { get; init; }
        public string Description { get; init; }
        public FinancialCategory Category { get; init; }
        public PaymentId? PaymentId { get; init; }
        public bool IsRelatedToPayment => PaymentId != null;

        public Transaction(Guid id, DateTime date, decimal amount, string description, FinancialCategory category, PaymentId? paymentId = null)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Description = description;
            Category = category;
            PaymentId = paymentId;
        }
    }
}