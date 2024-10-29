﻿using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Orders.Update
{
    public record UpdateOrderCommand(OrderId Id, string Name, string Description, DateOnly OrderDate, string OrderGroup,
        DateTime? Deadline, OrderStatus Status) : ICommand<OrderId>
    {

    }
}