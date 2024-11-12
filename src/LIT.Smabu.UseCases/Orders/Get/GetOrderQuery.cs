using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Orders.Get
{
    public record GetOrderQuery(OrderId OrderId) : IQuery<OrderDTO>
    {
    }
}