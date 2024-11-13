using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.Base
{
    public abstract class AggregateRoot<TEntityId> : Entity<TEntityId>, IAggregateRoot<TEntityId>
        where TEntityId : class, IEntityId
    {
        readonly List<IDomainEvent> _unhandledEvents = [];

        public AggregateMeta? Meta { get; set; }

        public void UpdateMeta(AggregateMeta aggregateMeta)
        {
            if (Meta == null || Meta.Version == aggregateMeta.Version - 1)
            {
                Meta = aggregateMeta;
            }
            else
            {
                throw new DomainException($"Expected version is {Meta.Version + 1} instead of {aggregateMeta.Version}.", Id);
            }
        }

        public virtual Result Delete()
        {
            return Result.Success();
        }

        public IEnumerable<IDomainEvent> GetUncommittedEvents(bool cleanup = true)
        {
            var events = _unhandledEvents.ToList();
            if (cleanup)
            {
                _unhandledEvents.Clear();
            }
            return events;
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _unhandledEvents.Add(domainEvent);
        }
    }
}
