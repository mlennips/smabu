using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Core
{
    public record DomainEventBase : IDomainEvent
    {
        public DateTime TriggeredAt { get; } = DateTime.UtcNow;
    }
}
