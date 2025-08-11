using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class UpdateCatalogItem
    {
        public record UpdateCatalogItemCommand(CatalogItemId CatalogItemId, CatalogId CatalogId, string Name, string Description,
            bool IsActive, Unit Unit, CatalogItemPrice[] Prices, CustomerCatalogItemPrice[] CustomerPrices) : ICommand;

        public class UpdateCatalogItemHandler(IAggregateRepository repository) : ICommandHandler<UpdateCatalogItemCommand>
        {

            public async Task<Result> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
            {
                Catalog catalog = await repository.GetByAsync(request.CatalogId);
                Result updateResult = catalog.UpdateItem(request.CatalogItemId, request.Name, request.Description, request.IsActive,
                    request.Unit, request.Prices, request.CustomerPrices);
                await repository.UpdateAsync(catalog);
                return updateResult;
            }
        }
    }
}
