using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Orders
{
    public class DeleteOrder
    {
        public record DeleteOrderCommand(OrderId OrderId) : ICommand;

        public class DeleteOrderHandler(IAggregateStore store) : ICommandHandler<DeleteOrderCommand>
        {
            public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var order = await store.GetByAsync(request.OrderId);
                order.Delete();
                await store.DeleteAsync(order);
                return Result.Success();
            }
        }
    }
}
