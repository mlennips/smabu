using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.UpdateOfferItem
{
    public class EditOfferItemHandler(IAggregateStore store) : ICommandHandler<UpdateOfferItemCommand>
    {
        public async Task<Result> Handle(UpdateOfferItemCommand request, CancellationToken cancellationToken)
        {
            var offer = await store.GetByAsync(request.OfferId);
            offer.UpdateItem(request.Id, request.Details, request.Quantity, request.UnitPrice, request.CatalogItemId);
            await store.UpdateAsync(offer);
            return Result.Success();
        }
    }
}
