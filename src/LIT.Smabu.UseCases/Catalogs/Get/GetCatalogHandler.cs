using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.Get
{
    public class GetCatalogHandler(IAggregateStore store) : IQueryHandler<GetCatalogQuery, CatalogDTO>
    {
        public async Task<Result<CatalogDTO>> Handle(GetCatalogQuery request, CancellationToken cancellationToken)
        {
            var catalog = await store.GetByAsync(CatalogId.DefaultId);
            return CatalogDTO.Create(catalog);
        }
    }
}
