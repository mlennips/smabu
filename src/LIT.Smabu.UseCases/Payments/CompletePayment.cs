using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Payments
{
    public static class CompletePayment
    {
        public record CompletePaymentCommand(PaymentId PaymentId, decimal Amount, DateTime PaidAt) : ICommand;

        public class CompletePaymentHandler(IAggregateStore store) : ICommandHandler<CompletePaymentCommand>
        {
            public async Task<Result> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
            {
                Payment payment = await store.GetByAsync(request.PaymentId);
                if (payment == null)
                {
                    return PaymentErrors.NotFound;
                }

                Result completeResult = payment.Complete(request.Amount, request.PaidAt);

                if (completeResult.IsSuccess)
                {
                    await store.UpdateAsync(payment);
                }

                return completeResult;
            }
        }
    }
}