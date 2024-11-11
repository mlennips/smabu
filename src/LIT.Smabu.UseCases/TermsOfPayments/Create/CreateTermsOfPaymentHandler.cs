using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.TermsOfPayments.Create
{
    public class CreateTermsOfPaymentHandler(IAggregateStore store) : ICommandHandler<CreateTermsOfPaymentCommand, TermsOfPaymentDTO>
    {
        public async Task<Result<TermsOfPaymentDTO>> Handle(CreateTermsOfPaymentCommand request, CancellationToken cancellationToken)
        {
            var termsOfPayment = TermsOfPayment.Create(request.Id, request.Title, request.Details, request.DueDays);
            await store.CreateAsync(termsOfPayment);
            return TermsOfPaymentDTO.Create(termsOfPayment);
        }
    }
}
