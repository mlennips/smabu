using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public record CatalogId(Guid Value) : EntityId<Catalog>(Value)
    {
        public static CatalogId DefaultId { get; } = new CatalogId(Guid.Parse("AAEE13EC-8D4C-439A-80C8-E591B7E0E1D8"));
    }
}