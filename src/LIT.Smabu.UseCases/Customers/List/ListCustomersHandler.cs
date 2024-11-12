using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Customers.List
{
    public class ListCustomersHandler(IAggregateStore store) : IQueryHandler<ListCustomersQuery, CustomerDTO[]>
    {
        public async Task<Result<CustomerDTO[]>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await store.GetAllAsync<Customer>();
            var result = customers.Select(x => CustomerDTO.Create(x))
                .OrderBy(x => x.Name)
                .ToArray();
            return result;
        }
    }
}
