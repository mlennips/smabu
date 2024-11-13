using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.CustomerAggregate;

namespace LIT.Smabu.UseCases.Offers
{
    public static class GetOffer
    {
        public record GetOfferQuery(OfferId OfferId, bool WithItems = false) : IQuery<OfferDTO>;

        public class GetOfferHandler(IAggregateStore store) : IQueryHandler<GetOfferQuery, OfferDTO>
        {
            public async Task<Result<OfferDTO>> Handle(GetOfferQuery request, CancellationToken cancellationToken)
            {
                Offer offer = await store.GetByAsync(request.OfferId);
                Customer customer = await store.GetByAsync(offer.CustomerId);
                return OfferDTO.Create(offer, customer, request.WithItems);
            }
        }
    }
}