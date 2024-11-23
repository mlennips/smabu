using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.CustomerAggregate;

namespace LIT.Smabu.UseCases.Orders
{
    public static class ListOrders
    {
        public record ListOrdersQuery(bool WithReferences = false) : IQuery<OrderDTO[]>;

        public class ListOrdersHandler(IAggregateStore store) : IQueryHandler<ListOrdersQuery, OrderDTO[]>
        {
            public async Task<Result<OrderDTO[]>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
            {
                IReadOnlyList<Order> orders = await store.GetAllAsync<Order>();
                Customer[] customers = await store.GetByAsync(orders.Select(x => x.CustomerId));
                IEnumerable<InvoiceId> invoiceIds = orders.SelectMany(x => x.References.InvoiceIds).Distinct();
                IEnumerable<OfferId> offerIds = orders.SelectMany(x => x.References.OfferIds).Distinct();

                List<Offer> offers = offerIds.Any()
                    ? [.. (await store.GetByAsync(offerIds))]
                    : [];

                List<Invoice> invoices = invoiceIds.Any()
                    ? [.. (await store.GetByAsync(invoiceIds))]
                    : [];

                return orders.Select
                    (
                        x => OrderDTO.Create(x, customers.Single(y => y.Id == x.CustomerId),
                        OrderReferencesDTO.Create(x.References, offers, invoices)
                    ))
                    .OrderByDescending(x => x.Number).ToArray();
            }
        }
    }
}
