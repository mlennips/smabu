namespace LIT.Smabu.Core
{
    public interface IAggregateRoot<out TEntityId> : IAggregateRoot, IEntity<TEntityId>
        where TEntityId : class, IEntityId
    {

    }

    public interface IAggregateRoot : IEntity
    {
        AggregateMeta? Meta { get; }
        void UpdateMeta(AggregateMeta aggregateMeta);
        IEnumerable<IDomainEvent> GetUncommittedEvents(bool cleanup = true);
    }
}

