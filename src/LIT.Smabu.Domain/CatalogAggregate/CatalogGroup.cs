using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public class CatalogGroup(CatalogGroupId id, CatalogId catalogId, string name, string description,
        List<CatalogItem> items) : Entity<CatalogGroupId>
    {
        private readonly List<CatalogItem> _items = items;

        public override CatalogGroupId Id { get; } = id;
        public override string DisplayName => Name;
        public CatalogId CatalogId { get; } = catalogId;
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public IReadOnlyList<CatalogItem> Items => _items;

        public static CatalogGroup Create(CatalogGroupId id, CatalogId catalogId, string name, string description)
        {
            return new CatalogGroup(id, catalogId, name, description, []);
        }

        public Result Update(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
            {
                return CatalogErrors.NameEmpty;
            }
            Name = name;
            Description = description;

            return Result.Success();
        }

        public Result<CatalogItem> AddItem(CatalogItemId id, CatalogItemNumber number, string name, string description, Unit defaultUnit)
        {
            if (string.IsNullOrEmpty(name))
            {
                return CatalogErrors.NameEmpty;
            }
            if (Items.Any(i => i.Name == name))
            {
                return CatalogErrors.NameAlreadyExists;
            }
            var item = CatalogItem.Create(id, number, CatalogId, Id, name, description, defaultUnit);
            _items.Add(item);
            return item;
        }

        public Result RemoveItem(CatalogItemId id)
        {
            CatalogItem? item = Items.SingleOrDefault(i => i.Id == id);
            if (item == null)
            {
                return Result.Failure(CatalogErrors.ItemNotFound);
            }
            _items.Remove(item);
            return Result.Success();
        }
    }
}
