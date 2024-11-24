using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public record CatalogGroupDTO(CatalogGroupId Id, CatalogId CatalogId, string DisplayName, string Name, string Description, CatalogItemDTO[] Items) : IDTO
    {
        internal static CatalogGroupDTO Create(CatalogGroup catalogGroup)
        {
            CatalogItemDTO[] items = catalogGroup.Items.Select(x => CatalogItemDTO.Create(x, catalogGroup)).ToArray();
            return new CatalogGroupDTO(catalogGroup.Id, catalogGroup.CatalogId, catalogGroup.DisplayName, catalogGroup.Name, catalogGroup.Description, items);
        }
    }
}