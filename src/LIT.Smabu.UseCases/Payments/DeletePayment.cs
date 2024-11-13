using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Payments
{
    public static class DeletePayment
    {
        public record DeletePaymentCommand(PaymentId PaymentId) : ICommand;

        public class DeletePaymentHandler(IAggregateStore store) : ICommandHandler<DeletePaymentCommand>
        {
            public async Task<Result> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
            {
                Payment payment = await store.GetByAsync(request.PaymentId);
                if (payment == null)
                {
                    return PaymentErrors.NotFound;
                }
                Result deleteResult = payment.Delete();
                if (deleteResult.IsSuccess)
                {
                    await store.DeleteAsync(payment);
                }
                return deleteResult;
            }
        }
    }
}