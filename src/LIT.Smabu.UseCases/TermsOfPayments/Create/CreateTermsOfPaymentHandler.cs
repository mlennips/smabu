using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.TermsOfPayments.Create
{
    public class CreateTermsOfPaymentHandler(IAggregateStore store) : ICommandHandler<CreateTermsOfPaymentCommand, TermsOfPaymentId>
    {
        public async Task<Result<TermsOfPaymentId>> Handle(CreateTermsOfPaymentCommand request, CancellationToken cancellationToken)
        {
            var termsOfPayment = TermsOfPayment.Create(request.TermsOfPaymentId, request.Title, request.Details, request.DueDays);
            await store.CreateAsync(termsOfPayment);
            return termsOfPayment.Id;
        }
    }
}
