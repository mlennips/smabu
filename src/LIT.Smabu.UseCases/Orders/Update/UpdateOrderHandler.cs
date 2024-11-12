using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Orders.Update
{
    public class UpdateOrderHandler(IAggregateStore store) : ICommandHandler<UpdateOrderCommand, OrderId>
    {
        public async Task<Result<OrderId>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await store.GetByAsync(request.OrderId);
            order.Update(request.Name, request.Description, request.OrderDate, request.BunchKey, request.Deadline);
            await store.UpdateAsync(order);
            return order.Id;
        }
    }
}
