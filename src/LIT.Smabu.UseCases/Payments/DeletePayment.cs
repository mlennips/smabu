using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.FinancialAggregate.Services;
using LIT.Smabu.Domain.FinancialAggregate;

namespace LIT.Smabu.UseCases.Payments
{
    public static class DeletePayment
    {
        public record DeletePaymentCommand(PaymentId PaymentId) : ICommand;

        public class DeletePaymentHandler(IAggregateStore store, FinancialRelationsService financialRelationsService) : ICommandHandler<DeletePaymentCommand>
        {
            public async Task<Result> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
            {
                Result financialResult = await financialRelationsService.CheckCanChangeRelatedItemAsync(request.PaymentId);
                if (financialResult.IsFailure)
                {
                    return financialResult;
                }

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