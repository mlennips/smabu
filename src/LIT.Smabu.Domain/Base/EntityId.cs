using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.Base
{
    public abstract record EntityId<TEntity>(Guid Value) : IEntityId<TEntity> where TEntity : IEntity
    {
        public sealed override string ToString()
        {
            return Value.ToString();
        }
    }
}
