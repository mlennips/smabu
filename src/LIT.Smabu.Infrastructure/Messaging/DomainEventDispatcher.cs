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

        public async Task HandleDomainEventsAsync<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregateRoot
        {
            var domainEvents = aggregate.GetUncommittedEvents();
            await HandleDomainEventsAsync([.. domainEvents]);
            _logger.LogInformation("Handled events for aggregate {type}/{id}", typeof(TAggregate).Name, aggregate.DisplayId);
        }

        public async Task HandleDomainEventsAsync(DomainEventBase[] domainEvents)
        {
            if (domainEvents.Length != 0)
            {
                foreach (var domainEvent in domainEvents.OrderBy(x => x.TriggeredAt))
                {
                    await _sender.Send(domainEvent);
                }
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
