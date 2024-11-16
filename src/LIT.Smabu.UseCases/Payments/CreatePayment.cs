using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Events;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.PaymentAggregate.Specifications;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using MediatR;

namespace LIT.Smabu.UseCases.Payments
{
    public static class CreatePayment
    {
        public record CreatePaymentCommand(PaymentId PaymentId, PaymentDirection Direction, string Details,
                string Payer, string Payee, CustomerId? CustomerId, InvoiceId? InvoiceId, string ReferenceNr, DateTime? ReferenceDate,
                decimal AmountDue, DateTime? DueDate, PaymentMethod PaymentMethod, PaymentCondition PaymentCondition, bool? MarkAsPaid = false) : ICommand<PaymentId>
        {
            internal static CreatePaymentCommand Create(Invoice invoice, Customer customer)
            {
                return new CreatePaymentCommand(
                    new PaymentId(Guid.NewGuid()),
                    PaymentDirection.Incoming,
                    "",
                    customer.Name,
                    "",
                    customer.Id,
                    invoice.Id,
                    invoice.Number.DisplayName,
                    invoice.InvoiceDate!.Value.ToDateTime(TimeOnly.MinValue),
                    invoice.Amount,
                    null,
                    customer.PreferredPaymentMethod,
                    customer.PaymentCondition,
                    false);
            }

            internal bool Validate()
            {
                return PaymentId != null && Direction != null
                    && (Direction != PaymentDirection.Incoming
                                        || CustomerId != null && InvoiceId != null)
                    && (Direction != PaymentDirection.Outgoing
                                        || CustomerId == null && InvoiceId == null);
            }
        }

        public class CreatePaymentHandler(IAggregateStore store, BusinessNumberService businessNumberService)
            : ICommandHandler<CreatePaymentCommand, PaymentId>, IRequestHandler<InvoiceReleasedEvent>
        {
            public async Task Handle(InvoiceReleasedEvent request, CancellationToken cancellationToken)
            {
                var alreadyExists = await CheckPaymentForInvoiceAlreadyExistsAsync(request.InvoiceId);
                if (alreadyExists)
                {
                    return;
                }
                Invoice invoice = await store.GetByAsync(request.InvoiceId);
                Customer customer = await store.GetByAsync(invoice.CustomerId);
                var command = CreatePaymentCommand.Create(invoice, customer);
                await Handle(command, cancellationToken);
            }

            public async Task<Result<PaymentId>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
            {
                if (!request.Validate())
                {
                    return PaymentErrors.InvalidCreate;
                }

                PaymentNumber number = await businessNumberService.CreatePaymentNumberAsync();

                Payment payment = request.Direction switch
                {
                    var direction when direction == PaymentDirection.Incoming
                        => Payment.CreateIncoming(request.PaymentId, number, request.Details, request.Payer, request.Payee,
                            request.CustomerId!, request.InvoiceId!, request.ReferenceNr, request.ReferenceDate,
                            request.AmountDue, request.DueDate, request.PaymentMethod, request.PaymentCondition),
                    var direction when direction == PaymentDirection.Outgoing
                        => Payment.CreateOutgoing(request.PaymentId, number, request.Details, request.Payer, request.Payee,
                            request.ReferenceNr, request.ReferenceDate,
                            request.AmountDue, request.DueDate, request.PaymentMethod, request.PaymentCondition),
                    _ => throw new InvalidOperationException($"Unknown payment direction: {request.Direction}")
                };

                if (request.MarkAsPaid.GetValueOrDefault())
                {
                    Result completeResult = payment.Complete(request.AmountDue, DateTime.UtcNow);
                    if (completeResult.IsFailure)
                    {
                        return completeResult.Error;
                    }
                }

                await store.CreateAsync(payment);
                return payment.Id;
            }

            private async Task<bool> CheckPaymentForInvoiceAlreadyExistsAsync(InvoiceId invoiceId)
            {
                IReadOnlyList<Payment> detectedPayments = await store.ApplySpecificationTask(new DetectPaymentsWithInvoiceIdSpec(invoiceId));
                return detectedPayments.Any();
            }
        }
    }
}