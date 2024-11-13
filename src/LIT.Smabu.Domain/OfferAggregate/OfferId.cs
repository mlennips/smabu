using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.OfferAggregate
{
    public record OfferId(Guid Value) : EntityId<Offer>(Value);
}
