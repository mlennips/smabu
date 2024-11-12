using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Get
{
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
