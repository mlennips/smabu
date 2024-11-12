using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;
using static LIT.Smabu.UseCases.Orders.OrderDTO;

namespace LIT.Smabu.UseCases.Orders.List
{
    public class ListOrdersHandler(IAggregateStore store) : IQueryHandler<ListOrdersQuery, OrderDTO[]>
    {
        public async Task<Result<OrderDTO[]>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await store.GetAllAsync<Order>();
            var customers = await store.GetByAsync(orders.Select(x => x.CustomerId).Distinct());
            var invoiceIds = orders.SelectMany(x => x.References.InvoiceIds).Distinct();
            var offerIds = orders.SelectMany(x => x.References.OfferIds).Distinct();

            var offers = offerIds.Any()
                ? (await store.GetByAsync(offerIds)).Values.ToList()
                : [];

            var invoices = invoiceIds.Any()
                ? (await store.GetByAsync(invoiceIds)).Values.ToList()
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
