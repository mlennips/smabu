using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public record CatalogGroupDTO(CatalogGroupId Id, CatalogId CatalogId, string Name, string Description, CatalogItemDTO[] Items) : IDTO
    {
        public string DisplayName => Name;

        internal static CatalogGroupDTO Create(CatalogGroup catalogGroup)
        {
            CatalogItemDTO[] items = catalogGroup.Items.Select(x => CatalogItemDTO.Create(x, catalogGroup)).ToArray();
            return new CatalogGroupDTO(catalogGroup.Id, catalogGroup.CatalogId, catalogGroup.Name, catalogGroup.Description, items);
        }
    }
}