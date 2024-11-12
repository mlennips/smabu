using LIT.Smabu.Shared;

namespace LIT.Smabu.Domain.Base
{
    public abstract class Entity<TEntityId> : IEntity<TEntityId> where TEntityId : IEntityId
    {
        public abstract TEntityId Id { get; }
    }
}
