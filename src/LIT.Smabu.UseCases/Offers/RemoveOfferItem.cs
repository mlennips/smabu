using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class RemoveOfferItem
    {
        public record RemoveOfferItemCommand(OfferItemId OfferItemId, OfferId OfferId) : ICommand<bool>;
        public class RemoveOfferItemHandler(IAggregateStore store) : ICommandHandler<RemoveOfferItemCommand, bool>
        {
            public async Task<Result<bool>> Handle(RemoveOfferItemCommand request, CancellationToken cancellationToken)
            {
                var offer = await store.GetByAsync(request.OfferId);
                offer.RemoveItem(request.OfferItemId);
                await store.UpdateAsync(offer);
                return true;
            }
        }
    }
}