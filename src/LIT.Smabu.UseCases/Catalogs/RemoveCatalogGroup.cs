using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class RemoveCatalogGroup
    {
        public record RemoveCatalogGroupCommand(CatalogGroupId CatalogGroupId, CatalogId CatalogId) : ICommand;
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
}
