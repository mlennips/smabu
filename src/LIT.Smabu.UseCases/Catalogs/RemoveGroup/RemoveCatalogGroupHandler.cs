using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.RemoveGroup
{
    public class RemoveCatalogGroupHandler(IAggregateStore store) : ICommandHandler<RemoveCatalogGroupCommand>
    {
        public async Task<Result> Handle(RemoveCatalogGroupCommand request, CancellationToken cancellationToken)
        {
            var catalog = await store.GetByAsync(request.CatalogId);
            var result = catalog.RemoveGroup(request.CatalogGroupId);
            await store.UpdateAsync(catalog);
            return result;
        }
    }
}
