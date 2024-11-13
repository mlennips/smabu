using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class AddCatalogItem
    {
        public record AddCatalogItemCommand(CatalogItemId CatalogItemId, CatalogId CatalogId, CatalogGroupId CatalogGroupId, string Name, string Description) : ICommand;

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
}
