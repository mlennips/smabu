using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate.Specifications;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.OrderAggregate.Services
{
    public class UpdateReferencesService(IUnitOfWork uow)
    {
        public async Task<Result> StartAsync(OrderId orderId, OrderReferences references)
        {
            Order order = await uow.Repository.GetByAsync(orderId);

            if (order == null)
            {
                return Result.Failure(OrderErrors.NotFound);
            }

            Result checkResult = await CheckReferencesAsync(orderId, references);
            if (checkResult.IsSuccess)
            {
                order.UpdateReferences(references);
                await uow.Repository.UpdateAsync(order);
            }
            return checkResult;
        }

        private async Task<Result> CheckReferencesAsync(OrderId orderId, OrderReferences references)
        {
            var errors = new List<ErrorDetail>();
            foreach (IEntityId entityId in references.GetAllReferenceIds())
            {
                Order? detectedOrder = (await uow.Repository.ApplySpecificationTask(new DetectOrderForReferenceIdSpec(entityId))).SingleOrDefault();
                if (detectedOrder != null && detectedOrder.Id != orderId)
                {
                    errors.Add(OrderErrors.ReferenceAlreadyAdded(entityId, detectedOrder.Number));
                }
            }
            return errors.Count != 0 ? Result.Failure(errors) : Result.Success();
        }
    }
}
