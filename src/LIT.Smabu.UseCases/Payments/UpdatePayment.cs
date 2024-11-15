using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Payments
{
    public static class UpdatePayment
    {
        public record UpdatePaymentCommand(PaymentId PaymentId, string Details, string Payer, string Payee, string ReferenceNr, DateTime? ReferenceDate,
                DateTime AccountingDate, decimal AmountDue, DateTime? DueDate, PaymentMethod PaymentMethod, PaymentStatus Status, PaymentCondition PaymentCondition) : ICommand;

        public class UpdatePaymentHandler(IAggregateStore store) : ICommandHandler<UpdatePaymentCommand>
        {
            public async Task<Result> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
            {
                Payment payment = await store.GetByAsync(request.PaymentId);
                if (payment == null)
                {
                    return PaymentErrors.NotFound;
                }

                Result updateResult = payment.Update(request.Details, request.Payer, request.Payee,
                    request.ReferenceNr, request.ReferenceDate, request.AmountDue, request.DueDate, request.PaymentMethod, request.Status, request.PaymentCondition);

                if (updateResult.IsSuccess)
                {
                    await store.UpdateAsync(payment);
                }

                return updateResult;
            }
        }
    }
}