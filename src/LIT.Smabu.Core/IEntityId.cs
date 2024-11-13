namespace LIT.Smabu.Core
{
    public interface IEntityId<out TEntity> : IEntityId where TEntity : IEntity
    {

    }

    public interface IEntityId : IValueObject
    {
        Guid Value { get; init; }
    }
}
