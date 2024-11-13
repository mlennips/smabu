using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Specifications;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.OfferAggregate.Specifications;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Orders
{
    public static class GetOrderReferences
    {
        public record GetOrderReferencesQuery(OrderId OrderId, bool OnlyForCustomer = true) : IQuery<GetOrderReferencesResponse>;

        public record GetOrderReferencesResponse(OrderReferenceDTO<OfferId>[] Offers, OrderReferenceDTO<InvoiceId>[] Invoices)
        {
            public decimal OfferAmount => Offers.Where(x => x.IsSelected ?? false).Sum(x => x.Amount ?? 0);
            public decimal InvoiceAmount => Invoices.Where(x => x.IsSelected ?? false).Sum(x => x.Amount ?? 0);
        }

        public class GetOrderReferencesHandler(IAggregateStore store) : IQueryHandler<GetOrderReferencesQuery, GetOrderReferencesResponse>
        {
            public async Task<Result<GetOrderReferencesResponse>> Handle(GetOrderReferencesQuery request, CancellationToken cancellationToken)
            {
                Order order = await store.GetByAsync(request.OrderId);

                IReadOnlyList<Offer> offers = request.OnlyForCustomer
                    ? await store.ApplySpecificationTask(new OffersByCustomerIdSpec(order.CustomerId))
                    : await store.GetAllAsync<Offer>();

                IReadOnlyList<Invoice> invoices = request.OnlyForCustomer
                    ? await store.ApplySpecificationTask(new InvoicesByCustomerIdSpec(order.CustomerId))
                    : await store.GetAllAsync<Invoice>();

                OrderReferenceDTO<OfferId>[] offerReferences = [.. offers.Select(x => new OrderReferenceDTO<OfferId>(
                    x.Id, x.Number.DisplayName, order.References.OfferIds.Contains(x.Id), x.OfferDate, x.Amount))
                    .OrderByDescending(x => x.Date)];

                OrderReferenceDTO<InvoiceId>[] invoiceReferences = [.. invoices.Select(x => new OrderReferenceDTO<InvoiceId>(
                    x.Id, x.Number.DisplayName, order.References.InvoiceIds.Contains(x.Id), x.InvoiceDate, x.Amount))
                    .OrderByDescending(x => x.Date)];

                return new GetOrderReferencesResponse(offerReferences, invoiceReferences);
            }
        }
    }
}
