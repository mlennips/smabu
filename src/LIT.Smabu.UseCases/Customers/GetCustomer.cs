using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Customers
{
    public static class GetCustomer
    {
        public record GetCustomerQuery(CustomerId CustomerId) : IQuery<CustomerDTO>;

        public class GetCustomerHandler(IAggregateCache cache) : IQueryHandler<GetCustomerQuery, CustomerDTO>
        {

            public async Task<Result<CustomerDTO>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
            {
                Customer customer = await cache.GetByAsync(request.CustomerId);
                var result = CustomerDTO.Create(customer);
                return result;
            }
        }
    }
}
