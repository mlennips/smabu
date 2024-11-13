using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class GetOffer
    {
        public record GetOfferQuery(OfferId OfferId, bool WithItems = false) : IQuery<OfferDTO>;

        public class GetOfferHandler(IAggregateStore store) : IQueryHandler<GetOfferQuery, OfferDTO>
        {
            public async Task<Result<OfferDTO>> Handle(GetOfferQuery request, CancellationToken cancellationToken)
            {
                var offer = await store.GetByAsync(request.OfferId);
                var customer = await store.GetByAsync(offer.CustomerId);
                return OfferDTO.Create(offer, customer, request.WithItems);
            }
        }
    }
}