﻿using LIT.Smabu.Shared;

namespace LIT.Smabu.Domain.OrderAggregate
{
    public record OrderId(Guid Value) : EntityId<Order>(Value);
}