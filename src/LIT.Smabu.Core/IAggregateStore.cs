namespace LIT.Smabu.Core
{
    public interface IAggregateStore
    {
        Task CreateAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;

        Task UpdateAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;

        Task DeleteAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>;

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
