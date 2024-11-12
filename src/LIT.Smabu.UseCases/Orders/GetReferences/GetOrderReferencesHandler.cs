using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Specifications;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.OfferAggregate.Specifications;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Orders.GetReferences
{
    public class GetOrderReferencesHandler(IAggregateStore store) : IQueryHandler<GetOrderReferencesQuery, GetOrderReferencesReadModel>
    {
        public async Task<Result<GetOrderReferencesReadModel>> Handle(GetOrderReferencesQuery request, CancellationToken cancellationToken)
        {
            var order = await store.GetByAsync(request.OrderId);

            var offers = request.OnlyForCustomer
                ? await store.ApplySpecificationTask(new OffersByCustomerIdSpec(order.CustomerId))
                : await store.GetAllAsync<Offer>();

            var invoices = request.OnlyForCustomer
                ? await store.ApplySpecificationTask(new InvoicesByCustomerIdSpec(order.CustomerId))
                : await store.GetAllAsync<Invoice>();

            var offerReferences = offers.Select(x => new OrderReferenceDTO<OfferId>(
                x.Id, x.Number.DisplayName, order.References.OfferIds.Contains(x.Id), x.OfferDate, x.Amount))
                .OrderByDescending(x => x.Date)
                .ToArray();

            var invoiceReferences = invoices.Select(x => new OrderReferenceDTO<InvoiceId>(
                x.Id, x.Number.DisplayName, order.References.InvoiceIds.Contains(x.Id), x.InvoiceDate, x.Amount))
                .OrderByDescending(x => x.Date)
                .ToArray();

            return new GetOrderReferencesReadModel(offerReferences, invoiceReferences);
        }
    }
}
