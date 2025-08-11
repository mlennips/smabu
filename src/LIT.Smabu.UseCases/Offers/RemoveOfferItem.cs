using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class RemoveOfferItem
    {
        public record RemoveOfferItemCommand(OfferItemId OfferItemId, OfferId OfferId) : ICommand<bool>;
        public class RemoveOfferItemHandler(IAggregateRepository repository) : ICommandHandler<RemoveOfferItemCommand, bool>
        {
            public async Task<Result<bool>> Handle(RemoveOfferItemCommand request, CancellationToken cancellationToken)
            {
                Offer offer = await repository.GetByAsync(request.OfferId);
                offer.RemoveItem(request.OfferItemId);
                await repository.UpdateAsync(offer);
                return true;
            }
        }
    }
}