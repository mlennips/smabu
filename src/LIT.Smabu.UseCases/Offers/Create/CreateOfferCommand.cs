using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Create
{
    public record CreateOfferCommand : ICommand<OfferId>
    {
        public required OfferId OfferId { get; set; }
        public required CustomerId CustomerId { get; set; }
        public required Currency Currency { get; set; }
        public TaxRate? TaxRate { get; set; }
        public OfferNumber? Number { get; set; }
    }
}
