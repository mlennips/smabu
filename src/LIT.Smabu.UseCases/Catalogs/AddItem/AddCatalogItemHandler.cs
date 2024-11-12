using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.AddItem
{
    public class AddCatalogItemHandler(IAggregateStore store) : ICommandHandler<AddCatalogItemCommand>
    {
        public async Task<Result> Handle(AddCatalogItemCommand request, CancellationToken cancellationToken)
        {
            var catalog = await store.GetByAsync(request.CatalogId);
            var addResult = catalog.AddItem(request.CatalogItemId, request.CatalogGroupId, request.Name, request.Description);
            await store.UpdateAsync(catalog);
            return addResult;
        }
    }
}
