using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.CustomerAggregate;

namespace LIT.Smabu.UseCases.Orders
{
    public static class GetOrder
    {
        public record GetOrderQuery(OrderId OrderId) : IQuery<OrderDTO>;

        public class GetOrderHandler(IAggregateStore store) : IQueryHandler<GetOrderQuery, OrderDTO>
        {
            public async Task<Result<OrderDTO>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
            {
                Order order = await store.GetByAsync(request.OrderId);
                Customer customer = await store.GetByAsync(order.CustomerId);

                List<Invoice> invoices = order.References.InvoiceIds.Count != 0
                    ? (await store.GetByAsync(order.References.InvoiceIds)).Select(x => x.Value).ToList()
                    : [];

                List<Offer> offers = order.References.OfferIds.Count != 0
                    ? (await store.GetByAsync(order.References.OfferIds)).Select(x => x.Value).ToList()
                    : [];

                var orderReferences = OrderReferencesDTO.Create(order.References, offers, invoices);

                return OrderDTO.Create(order, customer, orderReferences);
            }
        }
    }
}