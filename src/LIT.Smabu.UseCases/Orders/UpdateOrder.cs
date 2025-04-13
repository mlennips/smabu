using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Orders
{
    public static class UpdateOrder
    {
        public record UpdateOrderCommand(OrderId OrderId, string Name, string Description, DateOnly OrderDate, string BunchKey,
            DateTime? Deadline) : ICommand;

        public class UpdateOrderHandler(IAggregateStore store) : ICommandHandler<UpdateOrderCommand>
        {
            public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                Order order = await store.GetByAsync(request.OrderId);
                var result = order.Update(request.Name, request.Description, request.OrderDate, request.BunchKey, request.Deadline);
                if (result.IsSuccess)
                {
                    await store.UpdateAsync(order);
                }
                return result;
            }
        }
    }
}
