using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.CustomerAggregate;

namespace LIT.Smabu.UseCases.Offers
{
    public static class UpdateOffer
    {
        public record UpdateOfferCommand(OfferId OfferId, TaxRate TaxRate, DateOnly OfferDate, DateOnly ExpiresOn) : ICommand<OfferDTO>;

        public class UpdateOfferHandler(IUnitOfWork uow) : ICommandHandler<UpdateOfferCommand, OfferDTO>
        {
            public async Task<Result<OfferDTO>> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
            {
                Offer offer = await uow.Repository.GetByAsync(request.OfferId);
                Customer customer = await uow.Repository.GetByAsync(offer.CustomerId);
                offer.Update(request.TaxRate, request.OfferDate, request.ExpiresOn);
                await uow.Repository.UpdateAsync(offer);
                return OfferDTO.Create(offer, customer);
            }
        }
    }
}