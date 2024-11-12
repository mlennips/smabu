using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Payments
{
    public static class GetPayment
    {
        public record GetPaymentQuery(PaymentId PaymentId) : IQuery<PaymentDTO>;

        public class GetPaymentHandler(IAggregateStore store) : IQueryHandler<GetPaymentQuery, PaymentDTO>
        {
            public async Task<Result<PaymentDTO>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
            {
                var payment = await store.GetByAsync(request.PaymentId);
                if (payment == null)
                {
                    return PaymentErrors.NotFound;
                }
                return PaymentDTO.Create(payment);
            }
        }
    }
}