using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.RemoveOfferItem
{
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
