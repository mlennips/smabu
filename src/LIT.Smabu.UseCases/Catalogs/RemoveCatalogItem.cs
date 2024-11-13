using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.CatalogAggregate.Services;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class RemoveCatalogItem
    {
        public record RemoveCatalogItemCommand(CatalogId CatalogId, CatalogItemId CatalogItemId) : ICommand;

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
}
