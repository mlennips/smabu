using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class UpdateOffer
    {
        public record UpdateOfferCommand(OfferId OfferId, TaxRate TaxRate, DateOnly OfferDate, DateOnly ExpiresOn) : ICommand<OfferDTO>;

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
}