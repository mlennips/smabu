namespace LIT.Smabu.Core
{
    public interface IAggregateCache
    {
        Task<TAggregate[]> GetAllAsync<TAggregate>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;

        Task<TAggregate> GetByAsync<TAggregate>(IEntityId<TAggregate> id)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;

        Task<TAggregate[]> GetByAsync<TAggregate>(IEnumerable<IEntityId<TAggregate>> ids)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;

        Task<int> CountAsync<TAggregate>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;

        Task<TAggregate[]> ApplySpecificationTask<TAggregate>(Specification<TAggregate> specification)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;
    }
}