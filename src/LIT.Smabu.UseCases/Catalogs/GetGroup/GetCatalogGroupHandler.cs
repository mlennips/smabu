using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.GetGroup
{
    public class GetCatalogGroupHandler(IAggregateStore store) : IQueryHandler<GetCatalogGroupQuery, CatalogGroupDTO>
    {
        public async Task<Result<CatalogGroupDTO>> Handle(GetCatalogGroupQuery request, CancellationToken cancellationToken)
        {
            var catalog = await store.GetByAsync(request.CatalogId);
            var group = catalog.GetGroup(request.CatalogGroupId);
            return group != null
                ? CatalogGroupDTO.Create(group)
                : CatalogErrors.GroupNotFound;
        }
    }
}
