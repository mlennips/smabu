using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Specifications;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.PaymentAggregate.Specifications;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class ListInvoices
    {
        public record ListInvoicesQuery(CustomerId? CustomerId = null) : IQuery<ListInvoicesDTO[]>;

        public class ListInvoicesHandler(IAggregateStore store) : IQueryHandler<ListInvoicesQuery, ListInvoicesDTO[]>
        {
            public async Task<Result<ListInvoicesDTO[]>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
            {
                IReadOnlyList<Invoice> invoices = request.CustomerId != null
                    ? await store.ApplySpecificationTask(new InvoicesByCustomerIdSpec(request.CustomerId))
                    : await store.GetAllAsync<Invoice>();

                IReadOnlyList<Payment> payments = await store.ApplySpecificationTask(new PaymentsWithInvoiceIdSpec([.. invoices.Select(x => x.Id)]));
                var paidInvoices = payments.Where(x => x.InvoiceId != null).GroupBy(x => x.InvoiceId!)
                    .Where(x => x.All(y => y.Status == PaymentStatus.Paid))
                    .Select(x => x.Key).
                    ToHashSet();

                var customerIds = invoices.Select(x => x.CustomerId).ToList();
                Customer[] customers = await store.GetByAsync(customerIds);
                ListInvoicesDTO[] result = [.. invoices.Select(x => ListInvoicesDTO.Create(x, 
                    customers.Single(y => y.Id == x.CustomerId),
                    paidInvoices.Contains(x.Id)))
                    .OrderBy(x => x.Number.IsTemporary ? 0 : 1)
                    .ThenByDescending(x => x.Number)
                    .ThenByDescending(x => x.CreatedAt)];
                return result;
            }
        }

        public record ListInvoicesDTO : InvoiceDTO
        {
            public bool IsPaid { get; set; }

            protected ListInvoicesDTO(InvoiceDTO original, bool isPaid) : base(original)
            {
                IsPaid = isPaid;
            }

            public static ListInvoicesDTO Create(Invoice invoice, Customer customer, bool isPaid, bool withItems = false)
            {
                var original = InvoiceDTO.Create(invoice, customer, withItems);
                return new ListInvoicesDTO(original, isPaid);
            }
        }
    }
}