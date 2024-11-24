using LIT.Smabu.Core;
using LIT.Smabu.Infrastructure.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Infrastructure.Caching
{
    public class AggregateCache : IAggregateCache,
            IRequestHandler<InformativeNotification.AggregateCreatedEvent>,
            IRequestHandler<InformativeNotification.AggregateUpdatedEvent>,
            IRequestHandler<InformativeNotification.AggregateDeletedEvent>
    {
        public Task Handle(InformativeNotification.AggregateCreatedEvent request, CancellationToken cancellationToken)
        {
            // Handle the AggregateCreatedEvent
            Console.WriteLine($"Aggregate created: {request.Aggregate}");
            return Task.CompletedTask;
        }

        public Task Handle(InformativeNotification.AggregateUpdatedEvent request, CancellationToken cancellationToken)
        {
            // Handle the AggregateUpdatedEvent
            Console.WriteLine($"Aggregate updated: {request.Aggregate}");
            return Task.CompletedTask;
        }

        public Task Handle(InformativeNotification.AggregateDeletedEvent request, CancellationToken cancellationToken)
        {
            // Handle the AggregateDeletedEvent
            Console.WriteLine($"Aggregate deleted: {request.Aggregate}");
            return Task.CompletedTask;
        }
    }
}
