using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public record PaymentStatus : SimpleValueObject<string>
    {
        private static readonly HashSet<string> ValidValues = [PendingKey, PaidKey, CancelledKey, PartialKey];

        private const string PendingKey = "Pending";
        private const string PaidKey = "Paid";
        private const string CancelledKey = "Cancelled";
        private const string PartialKey = "Partial";

        public static PaymentStatus Pending { get; } = new PaymentStatus(PendingKey);
        public static PaymentStatus Partial { get; } = new PaymentStatus(PartialKey);
        public static PaymentStatus Paid { get; } = new PaymentStatus(PaidKey);
        public static PaymentStatus Cancelled { get; } = new PaymentStatus(CancelledKey);

        public PaymentStatus(string value) : base(value)
        {
            if (!ValidValues.Contains(value))
            {
                throw new ArgumentException("Invalid payment status value.");
            }
        }

        public static PaymentStatus[] GetAll() => ValidValues.Select(v => new PaymentStatus(v)).ToArray();
    }
}
