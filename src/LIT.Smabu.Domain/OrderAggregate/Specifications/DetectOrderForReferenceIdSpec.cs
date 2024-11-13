using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using System.Linq.Expressions;

namespace LIT.Smabu.Domain.OrderAggregate.Specifications
{
    public class DetectOrderForReferenceIdSpec : Specification<Order>
    {
        public DetectOrderForReferenceIdSpec(IEntityId referenceId) : base(GetExpression(referenceId))
        {
            OrderByDescendingExpression = x => x.Number.DisplayName;
            Take = 1;
        }

        private static Expression<Func<Order, bool>> GetExpression(IEntityId referenceId)
        {
            return referenceId is InvoiceId invoiceId
                ? (x => x.References.InvoiceIds.Contains(invoiceId))
                : (Expression<Func<Order, bool>>)(referenceId is OfferId offerId
                    ? (x => x.References.OfferIds.Contains(offerId))
                    : (x => false));
        }
    }
}
