using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OrderAggregate.Specifications;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.OfferAggregate.Services
{
    public class DeleteOfferService(IAggregateStore store)
    {
        public async Task<Result> DeleteAsync(OfferId id)
        {
            var hasRelations = await CheckIsOfferIdAsync(id);
            if (hasRelations)
            {
                return CommonErrors.HasReferences;
            }

            Offer invoice = await store.GetByAsync(id);
            invoice.Delete();
            await store.DeleteAsync(invoice);
            return Result.Success();
        }

        private async Task<bool> CheckIsOfferIdAsync(OfferId id)
        {
            IReadOnlyList<OrderAggregate.Order> orders = await store.ApplySpecificationTask(new DetectOrderForReferenceIdSpec(id));
            return orders.Any();
        }
    }
}
