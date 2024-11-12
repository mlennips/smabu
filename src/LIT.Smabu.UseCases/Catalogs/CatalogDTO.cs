using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public record CatalogDTO(CatalogId Id, string Name, CatalogGroupDTO[] Groups) : IDTO
    {
        public string DisplayName => Name;

        internal static Result<CatalogDTO> Create(Catalog catalog)
        {
            var groups = catalog.Groups.Select(x => CatalogGroupDTO.Create(x)).ToArray();
            return new CatalogDTO(catalog.Id, catalog.Name, groups);
        }
    }
}
