using LIT.Smabu.Domain.Base;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.OrderAggregate
{
    public static class OrderErrors
    {
        public static readonly ErrorDetail NotFound = new("Order.NotFound", "Order not found.");
        public static readonly ErrorDetail ReferenceNotFound = new("Order.ReferenceNotFound", "Reference not found.");

        public static ErrorDetail ReferenceAlreadyAdded(IEntityId entityId, OrderNumber number)
        {
            return new("Order.ReferenceAlreadyAdded", $"Reference '{entityId}' already added to order '{number.DisplayName}'.");
        }
    }
}
