
using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public static class CatalogErrors
    {
        public static readonly ErrorDetail NameEmpty = new("Catalog.NameEmpty", "Name must no be empty.");
        public static readonly ErrorDetail NameAlreadyExists = new("Catalog.NameAlreadyExists", "Name already exists.");
        public static readonly ErrorDetail UnitEmpty = new("Catalog.UnitEmpty", "Unit must no be empty.");
        public static readonly ErrorDetail NoValidPrice = new("Catalog.NoValidPrice", "One valid price must be available at least.");
        public static readonly ErrorDetail GroupNotFound = new("Catalog.GroupNotFound", "Group not found.");
        public static readonly ErrorDetail ItemNotFound = new("Catalog.ItemNotFound", "Item not found.");
        public static readonly ErrorDetail GroupNotEmpty = new("Catalog.GroupNotEmpty", "Group has items.");
    }
}
