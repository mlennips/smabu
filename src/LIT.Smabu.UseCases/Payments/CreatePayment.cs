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
using LIT.Smabu.Domain.FinancialAggregate.Services;

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

        public class CreatePaymentHandler(IAggregateStore store, BusinessNumberService businessNumberService, FinancialRelationsService financialRelationsService)
            : ICommandHandler<CreatePaymentCommand, PaymentId>
        {
            public async Task<Result<PaymentId>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
            {
                if (!request.Validate())
                {
                    return PaymentErrors.InvalidCreate;
                }

                CreatePaymentCommand updatedRequest = request with
                {
                    PaymentCondition = request.PaymentCondition ?? await DetectPaymentConditionAsync(request)
                };
                Payment payment = await CreatePaymentAsync(updatedRequest);

                if (request.MarkAsPaid.GetValueOrDefault())
                {
                    DateTime paidDate = DateTime.UtcNow;
                    Result financialResult = await financialRelationsService.CheckCanUseDateAsync(paidDate);
                    if (financialResult.IsFailure)
                    {
                        return financialResult.Error;
                    }

                    Result completeResult = payment.Complete(request.AmountDue, paidDate);
                    if (completeResult.IsFailure)
                    {
                        return completeResult.Error;
                    }
                }

                await store.CreateAsync(payment);
                return payment.Id;
            }

            private async Task<PaymentCondition> DetectPaymentConditionAsync(CreatePaymentCommand request)
            {
                PaymentCondition result = PaymentCondition.Default;
                if (request.CustomerId != null)
                {
                    Customer customer = await store.GetByAsync(request.CustomerId);
                    if (customer != null)
                    {
                        result = customer.PaymentCondition;
                    }
                }
                return result;
            }

            private async Task<Payment> CreatePaymentAsync(CreatePaymentCommand request)
            {
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
                return payment;
            }
        }
    }
}