using LIT.Smabu.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Infrastructure.Messaging
{
    public static class InformativeNotification
    {
        public interface IInformativeNotification : IRequest 
        {
            IAggregateRoot Aggregate { get; }
        }

        public sealed record AggregateCreatedEvent(IAggregateRoot Aggregate) : IInformativeNotification;
        public sealed record AggregateUpdatedEvent(IAggregateRoot Aggregate) : IInformativeNotification;
        public sealed record AggregateDeletedEvent(IAggregateRoot Aggregate) : IInformativeNotification;
    }
}
