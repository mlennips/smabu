using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Specifications;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.List
{
    public class ListInvoicesHandler(IAggregateStore store) : IQueryHandler<ListInvoicesQuery, InvoiceDTO[]>
    {
        public async Task<Result<InvoiceDTO[]>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Invoice> invoices = [];
            if (request.CustomerId != null)
            {
                invoices  = await store.ApplySpecificationTask(new InvoicesByCustomerIdSpec(request.CustomerId));
            }
            else
            {
                invoices = await store.GetAllAsync<Invoice>();
            }

            var customerIds = invoices.Select(x => x.CustomerId).ToList();
            var customers = await store.GetByAsync(customerIds);
            var result = invoices.Select(x => InvoiceDTO.Create(x, customers[x.CustomerId]))
                .OrderBy(x => x.Number.IsTemporary ? 0 : 1)
                .ThenByDescending(x => x.Number)
                .ThenByDescending(x => x.CreatedAt)
                .ToArray();
            return result;
        }
    }
}
