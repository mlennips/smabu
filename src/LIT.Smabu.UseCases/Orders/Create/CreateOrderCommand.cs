using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Orders.Create
{
    public record CreateOrderCommand(OrderId OrderId, CustomerId CustomerId, string Name, 
        DateTime OrderDate, OrderNumber? Number = null) : ICommand<OrderId>
    {

    }
}
