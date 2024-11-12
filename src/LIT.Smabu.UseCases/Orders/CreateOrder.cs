using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Orders
{
    public static class CreateOrder
    {
        public record CreateOrderCommand(OrderId OrderId, CustomerId CustomerId, string Name,
            DateTime OrderDate, OrderNumber? Number = null) : ICommand<OrderId>;

        public class CreateOrderHandler(IAggregateStore store, BusinessNumberService businessNumberService) : ICommandHandler<CreateOrderCommand, OrderId>
        {
            public async Task<Result<OrderId>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var number = request.Number ?? await businessNumberService.CreateOrderNumberAsync(request.OrderDate.Year);
                var order = Order.Create(request.OrderId, number, request.CustomerId, request.Name, DateOnly.FromDateTime(request.OrderDate));
                await store.CreateAsync(order);
                return order.Id;
            }
        }
    }
}
