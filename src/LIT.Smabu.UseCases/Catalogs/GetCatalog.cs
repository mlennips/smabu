using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class GetCatalog
    {
        public record GetCatalogQuery() : IQuery<CatalogDTO>;

        public class GetCatalogHandler(IAggregateStore store) : IQueryHandler<GetCatalogQuery, CatalogDTO>
        {
            public async Task<Result<CatalogDTO>> Handle(GetCatalogQuery request, CancellationToken cancellationToken)
            {
                Catalog catalog = await store.GetByAsync(CatalogId.DefaultId);
                return CatalogDTO.Create(catalog);
            }
        }
    }
}
