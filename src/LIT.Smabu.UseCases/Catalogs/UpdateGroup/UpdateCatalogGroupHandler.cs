using LIT.Smabu.Domain.Base;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.UpdateGroup
{
    public class UpdateCatalogGroupHandler(IAggregateStore store) : ICommandHandler<UpdateCatalogGroupCommand>
    {
        public async Task<Result> Handle(UpdateCatalogGroupCommand request, CancellationToken cancellationToken)
        {
            var catalog = await store.GetByAsync(request.CatalogId);
            var updateResult = catalog.UpdateGroup(request.CatalogGroupId, request.Name, request.Description);
            await store.UpdateAsync(catalog);
            return updateResult;
        }
    }
}
