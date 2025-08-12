namespace LIT.Smabu.Core
{
    public interface IAggregateRoot<out TEntityId> : IAggregateRoot, IEntity<TEntityId>
        where TEntityId : class, IEntityId
    {

    }

    public interface IAggregateRoot : IEntity
    {
        Guid DisplayId { get; }
        AggregateMeta? Meta { get; }
        void UpdateMeta(AggregateMeta aggregateMeta);
        IEnumerable<DomainEventBase> GetUncommittedEvents(bool cleanup = true);
        Result Validate();
    }
}

