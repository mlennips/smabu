using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Payments
{
    public static class GetPayment
    {
        public record GetPaymentQuery(PaymentId PaymentId) : IQuery<PaymentDTO>;

        public class GetPaymentHandler(IAggregateRepository repository) : IQueryHandler<GetPaymentQuery, PaymentDTO>
        {
            public async Task<Result<PaymentDTO>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
            {
                Payment payment = await repository.GetByAsync(request.PaymentId);
                return payment == null ? PaymentErrors.NotFound : PaymentDTO.Create(payment);
            }
        }
    }
}