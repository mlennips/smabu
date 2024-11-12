using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Get
{
    public record GetOfferQuery(OfferId OfferId) : IQuery<OfferDTO>
    {
        public bool WithItems { get; set; }
    }
}