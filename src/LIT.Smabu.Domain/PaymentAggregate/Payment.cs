﻿using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public class Payment : AggregateRoot<PaymentId>, IHasBusinessNumber<PaymentNumber>
    {
        public override PaymentId Id { get; }
        public override string DisplayName => $"{Number.DisplayName} {Payer} {Payee}".Trim();
        public PaymentNumber Number { get; }
        public PaymentDirection Direction { get; }
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
        public DateTime? AccountingDate => PaidAt;
        public Currency Currency { get; private set; }
        public PaymentStatus Status { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentCondition PaymentCondition { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Primären Konstruktor verwenden")]
        public Payment(PaymentId id, PaymentNumber number, PaymentDirection direction, string details, string payer, string payee,
            CustomerId? customerId, InvoiceId? invoiceId, string referenceNr, DateTime? referenceDate,
            decimal amountDue, DateTime? dueDate, decimal amountPaid, DateTime? paidAt, Currency currency, PaymentStatus status, PaymentMethod paymentMethod, PaymentCondition paymentCondition)
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
            AmountDue = amountDue;
            DueDate = dueDate;
            AmountPaid = amountPaid;
            PaidAt = paidAt;
            Currency = currency;
            Status = status;
            PaymentMethod = paymentMethod;
            PaymentCondition = paymentCondition;
        }

        public static Payment CreateIncoming(PaymentId id, PaymentNumber number, string details, string payer, string payee,
            CustomerId customerId, InvoiceId invoiceId, string referenceNr, DateTime? referenceDate,
            decimal amountDue, DateTime? dueDate, PaymentMethod paymentMethod, PaymentCondition paymentCondition)
        {
            if (dueDate == null && referenceDate != null)
            {
                dueDate = paymentCondition.CalculateLatestDueDate(referenceDate.Value);
            }
            return new Payment(id, number, PaymentDirection.Incoming, details, payer, payee, customerId, invoiceId,
                referenceNr, referenceDate, amountDue, dueDate, 0, null, Currency.EUR, PaymentStatus.Pending, paymentMethod, paymentCondition);
        }

        public static Payment CreateOutgoing(PaymentId id, PaymentNumber number, string details, string payer, string payee,
            string referenceNr, DateTime? referenceDate, decimal amountDue, DateTime? dueDate, PaymentMethod paymentMethod, PaymentCondition paymentCondition)
        {
            return new Payment(id, number, PaymentDirection.Outgoing, details, payer, payee, null, null,
                referenceNr, referenceDate, amountDue, dueDate, 0, null, Common.Currency.EUR, PaymentStatus.Pending, paymentMethod, paymentCondition);
        }

        public Result Update(string details, string payer, string payee, string referenceNr, DateTime? referenceDate,
            decimal amountDue, DateTime? dueDate, PaymentMethod paymentMethod, PaymentStatus status, PaymentCondition paymentCondition)
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
            DueDate = referenceDate != null ? paymentCondition.CalculateLatestDueDate(referenceDate.Value) : dueDate;
            Status = status;
            PaymentMethod = paymentMethod;
            PaymentCondition = paymentCondition;

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
            return Status == PaymentStatus.Paid ? (Result)PaymentErrors.PaymentAlreadyPaid : base.Delete();
        }

        public bool CheckIsOverdue(int toleranceDays = 2)
        {
            return Status == PaymentStatus.Pending && DueDate.HasValue && DueDate.Value.AddDays(toleranceDays) < DateTime.Now;
        }
    }
}
