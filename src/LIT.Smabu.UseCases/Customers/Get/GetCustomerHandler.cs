using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Customers.Get
{
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
