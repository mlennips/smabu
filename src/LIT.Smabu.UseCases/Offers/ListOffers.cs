using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class ListOffers
    {
        public record ListOffersQuery : IQuery<OfferDTO[]>;

        public class GetOffersHandler(IAggregateStore store) : IQueryHandler<ListOffersQuery, OfferDTO[]>
        {
            public async Task<Result<OfferDTO[]>> Handle(ListOffersQuery request, CancellationToken cancellationToken)
            {
                var offers = await store.GetAllAsync<Offer>();
                var customers = await store.GetByAsync(offers.Select(x => x.CustomerId).Distinct());
                return offers.Select(x => OfferDTO.Create(x, customers[x.CustomerId])).OrderByDescending(x => x.Number).ToArray();
            }
        }
    }
}