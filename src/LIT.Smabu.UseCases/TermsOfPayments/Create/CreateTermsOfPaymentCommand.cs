using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.TermsOfPayments.Create
{
    public record CreateTermsOfPaymentCommand : ICommand<TermsOfPaymentId>
    {
        public CreateTermsOfPaymentCommand(TermsOfPaymentId termsOfPaymentId, string title, string details, int? dueDays)
        {
            TermsOfPaymentId = termsOfPaymentId;
            Title = title;
            Details = details;
            DueDays = dueDays;
        }

        public TermsOfPaymentId TermsOfPaymentId { get; }
        public string Title { get; }
        public string Details { get; }
        public int? DueDays { get; }
    }
}
