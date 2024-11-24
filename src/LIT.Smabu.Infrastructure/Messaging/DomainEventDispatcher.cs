using LIT.Smabu.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using static LIT.Smabu.Infrastructure.Messaging.InformativeNotification;

namespace LIT.Smabu.Infrastructure.Messaging
{
    public class DomainEventDispatcher(ISender sender, ILogger<DomainEventDispatcher> logger) : IDomainEventDispatcher
    {
        private readonly ISender _sender = sender;
        private readonly ILogger<DomainEventDispatcher> _logger = logger;

        public async Task HandleDomainEventsAsync<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var domainEvents = aggregate.GetUncommittedEvents();
            if (domainEvents.Any())
            {
                foreach (var domainEvent in domainEvents)
                {
                    await _sender.Send(domainEvent);
                }
                _logger.LogInformation("Handled events for aggregate {type}/{id}", typeof(TAggregate).Name, aggregate.Id);
            }
        }

        async Task IDomainEventDispatcher.PublishInformativeCreatedNotificationAsync<TAggregate>(TAggregate aggregate)
        {
            await _sender.Send(new AggregateCreatedEvent(aggregate));
        }

        async Task IDomainEventDispatcher.PublishInformativeUpdatedNotificationEventAsync<TAggregate>(TAggregate aggregate)
        {
            await _sender.Send(new AggregateUpdatedEvent(aggregate));
        }

        async Task IDomainEventDispatcher.PublishInformativeDeletedNotificationAsync<TAggregate>(TAggregate aggregate)
        {
            await _sender.Send(new AggregateDeletedEvent(aggregate));
        }
    }
}
