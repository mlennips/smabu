using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.CustomerAggregate
{
    public record CustomerId(Guid Value) : EntityId<Customer>(Value);
}