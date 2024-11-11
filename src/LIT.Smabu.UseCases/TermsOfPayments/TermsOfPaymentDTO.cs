using LIT.Smabu.Domain.TermsOfPaymentAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.TermsOfPayments
{
    public record TermsOfPaymentDTO : IDTO
    {
        public TermsOfPaymentId Id { get; }
        public string Title { get; }
        public string Details { get; }
        public int? DueDays { get; }
        public string DisplayName => Title;

        public TermsOfPaymentDTO(TermsOfPaymentId id, string title, string details, int? dueDays)
        {
            Id = id;
            Title = title;
            Details = details;
            DueDays = dueDays;
        }

        public static TermsOfPaymentDTO Create(TermsOfPayment termsOfPayment)
        {
            return new(termsOfPayment.Id, termsOfPayment.Title, termsOfPayment.Details, termsOfPayment.DueDays);
        }
    }
}
