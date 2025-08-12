using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class UpdateOfferItem
    {
        public record UpdateOfferItemCommand(OfferItemId OfferItemId, OfferId OfferId, string Details,
            Quantity Quantity, decimal UnitPrice, CatalogItemId? CatalogItemId) : ICommand;

        public class EditOfferItemHandler(IUnitOfWork uow) : ICommandHandler<UpdateOfferItemCommand>
        {
            public async Task<Result> Handle(UpdateOfferItemCommand request, CancellationToken cancellationToken)
            {
                Offer offer = await uow.Repository.GetByAsync(request.OfferId);
                offer.UpdateItem(request.OfferItemId, request.Details, request.Quantity, request.UnitPrice, request.CatalogItemId);
                await uow.Repository.UpdateAsync(offer);
                return Result.Success();
            }
        }
    }
}