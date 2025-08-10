using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Customers
{
    public static class ListCustomer
    {
        public record ListCustomersQuery : IQuery<CustomerDTO[]>;

        public class ListCustomersHandler(IAggregateCache cache) : IQueryHandler<ListCustomersQuery, CustomerDTO[]>
        {
            public async Task<Result<CustomerDTO[]>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
            {
                IReadOnlyList<Customer> customers = await cache.GetAllAsync<Customer>();
                CustomerDTO[] result = [.. customers.Select(CustomerDTO.Create).OrderBy(x => x.Name)];
                return result;
            }
        }
    }
}
