﻿using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Update
{
    public class UpdateOfferHandler(IAggregateStore aggregateStore) : ICommandHandler<UpdateOfferCommand, OfferDTO>
    {
        public async Task<Result<OfferDTO>> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await aggregateStore.GetByAsync(request.Id);
            var customer = await aggregateStore.GetByAsync(offer.CustomerId);
            offer.Update(request.TaxRate, request.OfferDate, request.ExpiresOn);
            await aggregateStore.UpdateAsync(offer);
            return OfferDTO.Create(offer, customer);
        }
    }
}
