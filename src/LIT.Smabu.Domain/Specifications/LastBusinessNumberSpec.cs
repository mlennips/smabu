using LIT.Smabu.Domain.Base;
using LIT.Smabu.Core;
using System.Globalization;

namespace LIT.Smabu.Domain.Specifications
{
    public class LastBusinessNumberSpec<TAggregate, TNumber> : Specification<TAggregate>
            where TAggregate : IAggregateRoot<IEntityId<TAggregate>>, IHasBusinessNumber<TNumber>
            where TNumber : BusinessNumber
    {
        public LastBusinessNumberSpec()
            : base(x => true)
        {
            OrderByDescendingExpression = x => x.Number.DisplayName;
            Take = 1;
        }

        public LastBusinessNumberSpec(string year)
            : base(x => x.Number.Value.ToString(CultureInfo.InvariantCulture).StartsWith(year))
        {
            OrderByDescendingExpression = x => x.Number.DisplayName;
            Take = 1;
        }
    }
}
