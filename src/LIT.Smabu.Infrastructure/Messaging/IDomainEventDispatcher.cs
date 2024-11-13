using LIT.Smabu.Core;

namespace LIT.Smabu.Infrastructure.Messaging
{
    public interface IDomainEventDispatcher
    {
        Task HandleDomainEventsAsync<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;
    }
}