using System.Linq.Expressions;

namespace LIT.Smabu.Core
{
    public abstract class Specification<TAggregate>(Expression<Func<TAggregate, bool>> criteria)
        where TAggregate : IAggregateRoot<IEntityId<TAggregate>>
    {
        public Expression<Func<TAggregate, bool>> Criteria { get; } = criteria;
        public Expression<Func<TAggregate, object>>? OrderByExpression { get; protected set; }
        public Expression<Func<TAggregate, object>>? OrderByDescendingExpression { get; protected set; }
        public int? Take { get; protected set; }
    }
}
