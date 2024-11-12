using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public record CatalogGroupId(Guid Value) : EntityId<CatalogGroup>(Value);
}