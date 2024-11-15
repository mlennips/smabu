using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public record PaymentCondition : IValueObject
    {
        public record PaymentTerms(int DueDays, decimal DiscountPercentage);

        public string Name { get; }
        public PaymentTerms[] Terms { get; }

        public PaymentCondition(string name, PaymentTerms[] terms)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Payment condition must have a name");
            }
            if (terms.Length == 0)
            {
                throw new ArgumentException("Payment condition must have at least one term");
            }
            if (terms.Length > 1)
            {
                terms = [.. terms.OrderBy(t => t.DueDays)];
            }
            Name = name;
            Terms = terms;
        }

        public static PaymentCondition Default => Template14DaysNoDiscount;
        public static PaymentCondition Template14DaysNoDiscount => new("14 Tage netto ohne Abzug", [new PaymentTerms(14, 0)]);
        public static PaymentCondition Template30DaysNet14Days2PercentDiscount => new("30 Tage netto, 14 Tage 2% Skonto", [new PaymentTerms(14, 2), new PaymentTerms(30, 0)]);
        public static PaymentCondition TemplatePaymentOnInvoice => new("Zahlung bei Rechnungserhalt", [new PaymentTerms(0, 0)]);

        public DateTime CalculateLatestDueDate(DateTime referenceDate)
        {
            return referenceDate.AddDays(Terms[^1].DueDays);
        }

        public static PaymentCondition[] GetAll()
        {
            return [Template14DaysNoDiscount, Template30DaysNet14Days2PercentDiscount, TemplatePaymentOnInvoice];
        }
    }
}