using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.UpdateItem
{
    public class UpdateCatalogItemHandler(IAggregateStore store) : ICommandHandler<UpdateCatalogItemCommand>
    {
        public async Task<Result> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            var catalog = await store.GetByAsync(request.CatalogId);
            var updateResult = catalog.UpdateItem(request.CatalogItemId, request.Name, request.Description, request.IsActive,
                request.Unit, request.Prices, request.CustomerPrices);
            await store.UpdateAsync(catalog);
            return updateResult;
        }
    }
}
