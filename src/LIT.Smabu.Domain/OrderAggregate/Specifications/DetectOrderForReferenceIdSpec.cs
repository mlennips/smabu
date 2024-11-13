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
            if (referenceId is InvoiceId invoiceId)
            {
                return x => x.References.InvoiceIds.Contains(invoiceId);
            }
            else if (referenceId is OfferId offerId)
            {
                return x => x.References.OfferIds.Contains(offerId);
            }
            else
            {
                return x => false;
            }
        }
    }
}
