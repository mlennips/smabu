using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Payments.Update
{
    public class UpdatePaymentHandler(IAggregateStore store) : ICommandHandler<UpdatePaymentCommand>
    {
        public async Task<Result> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await store.GetByAsync(request.PaymentId);
            if (payment == null)
            {
                return PaymentErrors.NotFound;
            }
            var updateResult = payment.Update(request.Details, request.Payer, request.Payee, 
                request.ReferenceNr, request.ReferenceDate, request.AmountDue, request.DueDate, request.Status);

            if (updateResult.IsSuccess)
            {
                await store.UpdateAsync(payment);
            }

            return updateResult;
        }
    }
}
