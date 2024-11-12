using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.RemoveOfferItem
{
    public record RemoveOfferItemCommand(OfferItemId OfferItemId, OfferId OfferId) : ICommand<bool>
    {

    }
}
