﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Domain.Specifications;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Create
{
    public class CreateOfferHandler(IAggregateStore store, BusinessNumberService businessNumberService) : ICommandHandler<CreateOfferCommand, OfferId>
    {
        public async Task<Result<OfferId>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            var customer = await store.GetByAsync(request.CustomerId);
            var number = request.Number ?? await businessNumberService.CreateOfferNumberAsync();
            var offer = Offer.Create(request.Id, request.CustomerId, number, customer.MainAddress,
                request.Currency, request.TaxRate ?? TaxRate.Default);
            await store.CreateAsync(offer);
            return offer.Id;
        }
    }
}
