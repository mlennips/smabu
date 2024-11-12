using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class AddOfferItem
    {
        public record AddOfferItemCommand(OfferItemId OfferItemId, OfferId OfferId, string Details,
            Quantity Quantity, decimal UnitPrice, CatalogItemId? CatalogItemId) : ICommand<OfferItemId>;

        public class AddOfferItemHandler(IAggregateStore store) : ICommandHandler<AddOfferItemCommand, OfferItemId>
        {
            public async Task<Result<OfferItemId>> Handle(AddOfferItemCommand request, CancellationToken cancellationToken)
            {
                var offer = await store.GetByAsync(request.OfferId);
                offer.AddItem(request.OfferItemId, request.Details, request.Quantity, request.UnitPrice, request.CatalogItemId);
                await store.UpdateAsync(offer);
                return request.OfferItemId;
            }
        }
    }
}