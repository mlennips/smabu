using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Payments.Complete
{
    public class CompletePaymentHandler(IAggregateStore store) : ICommandHandler<CompletePaymentCommand>
    {
        public async Task<Result> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await store.GetByAsync(request.PaymentId);
            if (payment == null)
            {
                return PaymentErrors.NotFound;
            }
            var completeResult = payment.Complete(request.Amount, request.PaidAt);

            if (completeResult.IsSuccess)
            {
                await store.UpdateAsync(payment);
            }

            return completeResult;
        }
    }
}
