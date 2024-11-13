using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class UpdateCatalog
    {
        public record UpdateCatalogCommand(CatalogId CatalogId, string Name) : ICommand;

        public class UpdateCatalogHandler(IAggregateStore store) : ICommandHandler<UpdateCatalogCommand>
        {
            public async Task<Result> Handle(UpdateCatalogCommand request, CancellationToken cancellationToken)
            {
                var catalog = await store.GetByAsync(request.CatalogId);
                var updateResult = catalog.Update(request.Name);
                await store.UpdateAsync(catalog);
                return updateResult;
            }
        }
    }
}
