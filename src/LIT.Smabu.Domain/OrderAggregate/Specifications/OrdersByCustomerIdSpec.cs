using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.OrderAggregate.Specifications
{
    public class OrdersByCustomerIdSpec(CustomerId customerId)
        : Specification<Order>(x => x.CustomerId == customerId)
    {

    }
}
