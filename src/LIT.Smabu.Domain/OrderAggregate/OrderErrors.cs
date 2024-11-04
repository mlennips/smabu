﻿using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;

namespace LIT.Smabu.Domain.OrderAggregate
{
    public static class OrderErrors
    {
        public static readonly Error NotFound = new("Order.NotFound", "Order not found.");
        public static readonly Error ReferenceNotFound = new("Order.ReferenceNotFound", "Reference not found.");

        public static Error ReferenceAlreadyAdded(IEntityId entityId, OrderNumber number) =>
            new("Order.ReferenceAlreadyAdded", $"Reference '{entityId}' already added to order '{number.DisplayName}'.");

    }
}
