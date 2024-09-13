﻿using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.SeedWork;

namespace LIT.Smabu.UseCases.Offers.RemoveOfferItem
{
    public record RemoveOfferItemCommand(OfferItemId Id, OfferId OfferId) : ICommand<bool>
    {

    }
}
