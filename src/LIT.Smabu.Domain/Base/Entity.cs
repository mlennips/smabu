using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.Base
{
    public abstract class Entity<TEntityId> : IEntity<TEntityId>, IHasDisplayName where TEntityId : IEntityId
    {
        public abstract TEntityId Id { get; }

        public abstract string DisplayName { get; }
    }
}
