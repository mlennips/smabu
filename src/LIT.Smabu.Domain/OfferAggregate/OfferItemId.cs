using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.OfferAggregate
{
    public record OfferItemId(Guid Value) : EntityId<OfferItem>(Value);
}