using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Update
{
    public record UpdateOfferCommand : ICommand<OfferDTO>
    {
        public required OfferId OfferId { get; set; }
        public required TaxRate TaxRate { get; set; }
        public DateOnly OfferDate { get; set; }
        public DateOnly ExpiresOn { get; set; }
    }
}
