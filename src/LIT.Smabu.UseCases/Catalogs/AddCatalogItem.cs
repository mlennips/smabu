using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.Domain.InvoiceAggregate;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class AddCatalogItem
    {
        public record AddCatalogItemCommand(CatalogItemId CatalogItemId, CatalogId CatalogId, CatalogGroupId CatalogGroupId, string Name, string Description) : ICommand;

        public class AddCatalogItemHandler(IAggregateRepository repository) : ICommandHandler<AddCatalogItemCommand>
        {
            public async Task<Result> Handle(AddCatalogItemCommand request, CancellationToken cancellationToken)
            {
                Catalog catalog = await repository.GetByAsync(request.CatalogId);
                Result<CatalogItem> addResult = catalog.AddItem(request.CatalogItemId, request.CatalogGroupId, request.Name, request.Description);
                await repository.UpdateAsync(catalog);
                return addResult;
            }
        }
    }
}
