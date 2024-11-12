using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Update
{
    public class UpdateOfferHandler(IAggregateStore store) : ICommandHandler<UpdateOfferCommand, OfferDTO>
    {
        public async Task<Result<OfferDTO>> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await store.GetByAsync(request.OfferId);
            var customer = await store.GetByAsync(offer.CustomerId);
            offer.Update(request.TaxRate, request.OfferDate, request.ExpiresOn);
            await store.UpdateAsync(offer);
            return OfferDTO.Create(offer, customer);
        }
    }
}
