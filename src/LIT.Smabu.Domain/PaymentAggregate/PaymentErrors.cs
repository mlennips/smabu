using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public static class PaymentErrors
    {
        public static ErrorDetail StatusMustNotBeNull => new("Payment.StatusIsNull", "Status must not be null.");
        public static ErrorDetail PaymentAlreadyPaid => new("Payment.AlreadyPaid", "Payment is already paid.");
        public static ErrorDetail InvalidCreate => new("Payment.InvalidCreate", "Arguments for creation are invalid.");
        public static ErrorDetail NotFound => new("Payment.NotFound", "Payment not found.");
    }
}
