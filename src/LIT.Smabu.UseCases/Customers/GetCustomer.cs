using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Customers
{
    public static class GetCustomer
    {
        public record GetCustomerQuery(CustomerId CustomerId) : IQuery<CustomerDTO>;

        public class GetCustomerHandler(IAggregateStore store) : IQueryHandler<GetCustomerQuery, CustomerDTO>
        {

            public async Task<Result<CustomerDTO>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
            {
                var customer = await store.GetByAsync(request.CustomerId);
                var result = CustomerDTO.Create(customer);
                return result;
            }
        }
    }
}
