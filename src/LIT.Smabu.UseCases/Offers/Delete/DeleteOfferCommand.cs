using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Delete
{
    public record DeleteOfferCommand(OfferId OfferId) : ICommand
    {

    }
}
