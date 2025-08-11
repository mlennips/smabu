using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Orders
{
    public class DeleteOrder
    {
        public record DeleteOrderCommand(OrderId OrderId) : ICommand;

        public class DeleteOrderHandler(IAggregateRepository repository) : ICommandHandler<DeleteOrderCommand>
        {
            public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                Order order = await repository.GetByAsync(request.OrderId);
                order.Delete();
                await repository.DeleteAsync(order);
                return Result.Success();
            }
        }
    }
}
