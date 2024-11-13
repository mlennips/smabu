using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.OrderAggregate
{
    public record OrderReferences(HashSet<OfferId> OfferIds, HashSet<InvoiceId> InvoiceIds) : IValueObject
    {
        public static OrderReferences Empty => new([], []);

        public IEnumerable<IEntityId> GetAllReferenceIds() => OfferIds.Cast<IEntityId>().Concat(InvoiceIds.Cast<IEntityId>());
    }
}
