using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.OrderAggregate
{
    public record OrderId(Guid Value) : EntityId<Order>(Value);
}