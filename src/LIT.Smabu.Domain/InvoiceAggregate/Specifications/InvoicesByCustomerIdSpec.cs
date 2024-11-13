using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.InvoiceAggregate.Specifications
{
    public class InvoicesByCustomerIdSpec(CustomerId customerId)
        : Specification<Invoice>(x => x.CustomerId.Value == customerId.Value)
    {

    }
}
