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

        public class GetOrderHandler(IAggregateCache cache) : IQueryHandler<GetOrderQuery, OrderDTO>
        {
            public async Task<Result<OrderDTO>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
            {
                Order order = await cache.GetByAsync(request.OrderId);
                Customer customer = await cache.GetByAsync(order.CustomerId);

                List<Invoice> invoices = order.References.InvoiceIds.Count != 0
                    ? [.. (await cache.GetByAsync(order.References.InvoiceIds))]
                    : [];

                List<Offer> offers = order.References.OfferIds.Count != 0
                    ? [.. (await cache.GetByAsync(order.References.OfferIds))]
                    : [];

                var orderReferences = OrderReferencesDTO.Create(order.References, offers, invoices);

                return OrderDTO.Create(order, customer, orderReferences);
            }
        }
    }
}