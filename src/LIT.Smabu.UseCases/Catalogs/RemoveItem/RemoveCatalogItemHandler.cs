using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate.Services;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.RemoveItem
{
    public class RemoveCatalogItemHandler(RemoveCatalogItemService removeCatalogItemService) : ICommandHandler<RemoveCatalogItemCommand>
    {
        public async Task<Result> Handle(RemoveCatalogItemCommand request, CancellationToken cancellationToken)
        {
            var result = await removeCatalogItemService.RemoveAsync(request.CatalogId, request.CatalogItemId);
            if (result.IsFailure)
            {
                return result.Error;
            }
            return Result.Success();
        }
    }
}
