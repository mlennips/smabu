﻿using LIT.Smabu.Domain.Contracts;

namespace LIT.Smabu.Domain.OfferAggregate
{
    public record OfferId(Guid Value) : EntityId<Offer>(Value);
}
