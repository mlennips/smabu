using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public class Payment : AggregateRoot<PaymentId>, IHasBusinessNumber<PaymentNumber>
    {
        public override PaymentId Id { get; }
        public PaymentNumber Number { get; }
        public PaymentDirection Direction { get; }
        public DateTime AccountingDate { get; private set; }
        public string Details { get; private set; }
        public string Payer { get; private set; }
        public string Payee { get; private set; }
        public CustomerId? CustomerId { get; private set; }
        public InvoiceId? InvoiceId { get; private set; }
        public string ReferenceNr { get; private set; }
        public DateTime? ReferenceDate { get; private set; }
        public decimal AmountDue { get; private set; }
        public DateTime? DueDate { get; private set; }
        public decimal AmountPaid { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public Currency Currency { get; private set; }
        public PaymentStatus Status { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Primären Konstruktor verwenden")]
        public Payment(PaymentId id, PaymentNumber number, PaymentDirection direction, DateTime accountingDate, string details, string payer, string payee,
            CustomerId? customerId, InvoiceId? invoiceId, string referenceNr, DateTime? referenceDate,
            decimal amountDue, DateTime? dueDate, decimal amountPaid, DateTime? paidAt, Currency currency, PaymentStatus status)
        {
            Id = id;
            Number = number;
            Direction = direction;
            Details = details;
            Payer = payer;
            Payee = payee;
            CustomerId = customerId;
            InvoiceId = invoiceId;
            ReferenceNr = referenceNr;
            ReferenceDate = referenceDate;
            AccountingDate = accountingDate;
            AmountDue = amountDue;
            DueDate = dueDate;
            AmountPaid = amountPaid;
            PaidAt = paidAt;
            Currency = currency;
            Status = status;
        }

        public static Payment CreateIncoming(PaymentId id, PaymentNumber number, string details, string payer, string payee,
            CustomerId customerId, InvoiceId invoiceId, string referenceNr, DateTime? referenceDate, DateTime accountingDate, 
            decimal amountDue, DateTime? dueDate)
        {
            return new Payment(id, number, PaymentDirection.Incoming, accountingDate, details, payer, payee, customerId, invoiceId,
                referenceNr, referenceDate, amountDue, dueDate, 0, null, Currency.EUR, PaymentStatus.Pending);
        }

        public static Payment CreateOutgoing(PaymentId id, PaymentNumber number, string details, string payer, string payee,
            string referenceNr, DateTime? referenceDate, DateTime accountingDate, decimal amountDue, DateTime? dueDate)
        {
            return new Payment(id, number, PaymentDirection.Outgoing, accountingDate, details, payer, payee, null, null,
                referenceNr, referenceDate, amountDue, dueDate, 0, null, Common.Currency.EUR, PaymentStatus.Pending);
        }

        public Result Update(string details, string payer, string payee, string referenceNr, DateTime? referenceDate,
            decimal amountDue, DateTime? dueDate, PaymentStatus status)
        {
            if (status == PaymentStatus.Paid && Status == PaymentStatus.Paid)
            {
                return PaymentErrors.PaymentAlreadyPaid;
            }
            if (status == null)
            {
                return PaymentErrors.StatusMustNotBeNull;
            }

            Details = details;
            Payer = payer;
            Payee = payee;
            ReferenceNr = referenceNr;
            ReferenceDate = referenceDate;
            AmountDue = amountDue;
            DueDate = dueDate;
            Status = status;

            return Result.Success();
        }

        public Result Complete(decimal amountPaid, DateTime paidAt)
        {
            if (Status == PaymentStatus.Paid)
            {
                return PaymentErrors.PaymentAlreadyPaid;
            }

            AmountPaid = amountPaid;
            PaidAt = paidAt;
            Status = PaymentStatus.Paid;

            return Result.Success();
        }

        public override Result Delete()
        {
            if (Status == PaymentStatus.Paid)
            {
                return PaymentErrors.PaymentAlreadyPaid;
            }
            return base.Delete();
        }

        public bool CheckIsOverdue(int toleranceDays = 2)
        {
            return Status == PaymentStatus.Pending && DueDate.HasValue && DueDate.Value.AddDays(toleranceDays) < DateTime.Now;
        }
    }
}
