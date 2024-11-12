using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Orders.Update
{
    public record UpdateOrderCommand(OrderId OrderId, string Name, string Description, DateOnly OrderDate, string BunchKey,
        DateTime? Deadline) : ICommand<OrderId>
    {

    }
}
