using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate.Specifications;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.OrderAggregate.Services
{
    public class UpdateReferencesService(IAggregateStore store)
    {
        public async Task<Result> StartAsync(OrderId orderId, OrderReferences references)
        {
            var order = await store.GetByAsync(orderId);

            if (order == null)
            {
                return Result.Failure(OrderErrors.NotFound);
            }

            var checkResult = await CheckReferencesAsync(orderId, references);
            if (checkResult.IsSuccess)
            {
                order.UpdateReferences(references);
                await store.UpdateAsync(order);
            }
            return checkResult;
        }

        private async Task<Result> CheckReferencesAsync(OrderId orderId, OrderReferences references)
        {
            var errors = new List<ErrorDetail>();
            foreach (var entityId in references.GetAllReferenceIds())
            {
                var detectedOrder = (await store.ApplySpecificationTask(new DetectOrderForReferenceIdSpec(entityId))).SingleOrDefault();
                if (detectedOrder != null && detectedOrder.Id != orderId)
                {
                    errors.Add(OrderErrors.ReferenceAlreadyAdded(entityId, detectedOrder.Number));
                }
            }
            if (errors.Count != 0)
            {
                return Result.Failure(errors);
            }
            else
            {
                return Result.Success();
            }
        }
    }
}
