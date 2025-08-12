using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate.Specifications;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.OrderAggregate.Services
{
    public class UpdateReferencesService(IAggregateRepository repository)
    {
        public async Task<Result> StartAsync(OrderId orderId, OrderReferences references)
        {
            Order order = await repository.GetByAsync(orderId);

            if (order == null)
            {
                return Result.Failure(OrderErrors.NotFound);
            }

            Result checkResult = await CheckReferencesAsync(orderId, references);
            if (checkResult.IsSuccess)
            {
                order.UpdateReferences(references);
                await repository.UpdateAsync(order);
            }
            return checkResult;
        }

        private async Task<Result> CheckReferencesAsync(OrderId orderId, OrderReferences references)
        {
            var errors = new List<ErrorDetail>();
            foreach (IEntityId entityId in references.GetAllReferenceIds())
            {
                Order? detectedOrder = (await repository.ApplySpecificationTask(new DetectOrderForReferenceIdSpec(entityId))).SingleOrDefault();
                if (detectedOrder != null && detectedOrder.Id != orderId)
                {
                    errors.Add(OrderErrors.ReferenceAlreadyAdded(entityId, detectedOrder.Number));
                }
            }
            return errors.Count != 0 ? Result.Failure(errors) : Result.Success();
        }
    }
}
