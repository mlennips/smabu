using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;

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
                Dictionary<IEntityId<Domain.CustomerAggregate.Customer>, Domain.CustomerAggregate.Customer> customers
                    = await store.GetByAsync(orders.Select(x => x.CustomerId).Distinct());
                IEnumerable<InvoiceId> invoiceIds = orders.SelectMany(x => x.References.InvoiceIds).Distinct();
                IEnumerable<OfferId> offerIds = orders.SelectMany(x => x.References.OfferIds).Distinct();

                List<Offer> offers = offerIds.Any()
                    ? [.. (await store.GetByAsync(offerIds)).Values]
                    : [];

                List<Invoice> invoices = invoiceIds.Any()
                    ? [.. (await store.GetByAsync(invoiceIds)).Values]
                    : [];

                return orders.Select
                    (
                        x => OrderDTO.Create(x, customers[x.CustomerId],
                        OrderReferencesDTO.Create(x.References, offers, invoices)
                    ))
                    .OrderByDescending(x => x.Number).ToArray();
            }
        }
    }
}
