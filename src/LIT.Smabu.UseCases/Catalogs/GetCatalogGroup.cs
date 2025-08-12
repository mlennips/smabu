using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class GetCatalogGroup
    {
        public record GetCatalogGroupQuery(CatalogGroupId CatalogGroupId, CatalogId CatalogId) : IQuery<CatalogGroupDTO>;

        public class GetCatalogGroupHandler(IAggregateRepository repository) : IQueryHandler<GetCatalogGroupQuery, CatalogGroupDTO>
        {
            public async Task<Result<CatalogGroupDTO>> Handle(GetCatalogGroupQuery request, CancellationToken cancellationToken)
            {
                Catalog catalog = await repository.GetByAsync(request.CatalogId);
                CatalogGroup? group = catalog.GetGroup(request.CatalogGroupId);
                return group != null
                    ? CatalogGroupDTO.Create(group)
                    : CatalogErrors.GroupNotFound;
            }
        }
    }
}
