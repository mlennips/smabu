using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.TermsOfPayments
{
    public static class CreateTermsOfPayment
    {
        public record CreateTermsOfPaymentCommand(TermsOfPaymentId TermsOfPaymentId, string Title, string Details, int? DueDays) : ICommand<TermsOfPaymentId>;

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
}
