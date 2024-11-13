using LIT.Smabu.Core;

namespace LIT.Smabu.Infrastructure.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TAggregate> GetQuery<TAggregate>(
            IQueryable<TAggregate> inputQueryable,
            Specification<TAggregate> specification)
            where TAggregate : IAggregateRoot<IEntityId<TAggregate>>
        {
            IQueryable<TAggregate> queryable = inputQueryable;
            queryable = queryable.Where(specification.Criteria);

            if (specification.OrderByExpression != null)
            {
                queryable = queryable.OrderBy(specification.OrderByExpression);
            }
            else if (specification.OrderByDescendingExpression != null)
            {
                queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
            }
            if (specification.Take != null)
            {
                queryable = queryable.Take(specification.Take.Value);
            }

            return queryable;
        }
    }
}
