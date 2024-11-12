using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Payments.Update
{
    public record UpdatePaymentCommand : ICommand
    {
        public PaymentId PaymentId { get; }
        public string Details { get; }
        public string Payer { get; }
        public string Payee { get; }
        public string ReferenceNr { get; }
        public DateTime? ReferenceDate { get; }
        public DateTime AccountingDate { get; }
        public decimal AmountDue { get; }
        public DateTime? DueDate { get; }
        public PaymentStatus Status { get; }

        public UpdatePaymentCommand(PaymentId paymentId, string details, string payer, string payee, string referenceNr, DateTime? referenceDate, 
            DateTime accountingDate, decimal amountDue, PaymentStatus status)
        {
            PaymentId = paymentId;
            Details = details;
            Payer = payer;
            Payee = payee;
            ReferenceNr = referenceNr;
            ReferenceDate = referenceDate;
            AccountingDate = accountingDate;
            AmountDue = amountDue;
            Status = status;
        }
    }
}
