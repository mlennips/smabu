﻿using LIT.Smabu.Domain.Shared;

namespace LIT.Smabu.Domain.OfferAggregate
{
    public record OfferId(Guid Value) : EntityId<Offer>(Value);
}
