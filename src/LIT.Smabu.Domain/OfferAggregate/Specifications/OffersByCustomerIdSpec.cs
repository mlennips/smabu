using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.OfferAggregate.Specifications
{
    public class OffersByCustomerIdSpec(CustomerId customerId)
        : Specification<Offer>(x => x.CustomerId == customerId)
    {

    }
}
