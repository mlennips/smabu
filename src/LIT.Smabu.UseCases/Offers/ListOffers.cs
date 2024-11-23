using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.CustomerAggregate;

namespace LIT.Smabu.UseCases.Offers
{
    public static class ListOffers
    {
        public record ListOffersQuery : IQuery<OfferDTO[]>;

        public class GetOffersHandler(IAggregateStore store) : IQueryHandler<ListOffersQuery, OfferDTO[]>
        {
            public async Task<Result<OfferDTO[]>> Handle(ListOffersQuery request, CancellationToken cancellationToken)
            {
                IReadOnlyList<Offer> offers = await store.GetAllAsync<Offer>();
                Customer[] customers = await store.GetByAsync(offers.Select(x => x.CustomerId));
                return offers.Select(x => OfferDTO.Create(x, customers.Single(y => y.Id == x.CustomerId))).OrderByDescending(x => x.Number).ToArray();
            }
        }
    }
}