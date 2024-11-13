using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public record CatalogItemId(Guid Value) : EntityId<CatalogItem>(Value);
}