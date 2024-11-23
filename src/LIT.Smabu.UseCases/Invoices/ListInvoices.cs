using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Specifications;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class ListInvoices
    {
        public record ListInvoicesQuery(CustomerId? CustomerId = null) : IQuery<InvoiceDTO[]>;

        public class ListInvoicesHandler(IAggregateStore store) : IQueryHandler<ListInvoicesQuery, InvoiceDTO[]>
        {
            public async Task<Result<InvoiceDTO[]>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
            {
                IReadOnlyList<Invoice> invoices = [];
                invoices = request.CustomerId != null
                    ? await store.ApplySpecificationTask(new InvoicesByCustomerIdSpec(request.CustomerId))
                    : await store.GetAllAsync<Invoice>();

                var customerIds = invoices.Select(x => x.CustomerId).ToList();
                Customer[] customers = await store.GetByAsync(customerIds);
                InvoiceDTO[] result = [.. invoices.Select(x => InvoiceDTO.Create(x, customers.Single(y => y.Id == x.CustomerId)))
                    .OrderBy(x => x.Number.IsTemporary ? 0 : 1)
                    .ThenByDescending(x => x.Number)
                    .ThenByDescending(x => x.CreatedAt)];
                return result;
            }
        }
    }
}