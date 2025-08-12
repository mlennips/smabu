using LIT.Smabu.Core;

namespace LIT.Smabu.Infrastructure.Messaging
{
    public interface IDomainEventDispatcher
    {
        Task HandleDomainEventsAsync<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregateRoot;
        Task HandleDomainEventsAsync(DomainEventBase[] domainEvents);
        Task PublishInformativeCreatedNotificationAsync<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregateRoot;
        Task PublishInformativeDeletedNotificationAsync<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregateRoot;
        Task PublishInformativeUpdatedNotificationEventAsync<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregateRoot;
    }
}