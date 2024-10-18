﻿using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Customers.Get
{
    public record GetCustomerQuery : IQuery<CustomerDTO>
    {
        public GetCustomerQuery(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        public CustomerId CustomerId { get; }
    }
}
