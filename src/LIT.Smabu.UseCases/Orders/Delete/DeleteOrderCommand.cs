using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Orders.Delete
{
    public record DeleteOrderCommand(OrderId OrderId) : ICommand
    {

    }
}
