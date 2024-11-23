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

        public LastBusinessNumberSpec(int year)
#pragma warning disable CA1305 // Specify IFormatProvider
            : base(x => x.Number.Value.ToString().StartsWith(year.ToString()))
#pragma warning restore CA1305 // Specify IFormatProvider
        {
            OrderByDescendingExpression = x => x.Number.DisplayName;
            Take = 1;
        }
    }
}
