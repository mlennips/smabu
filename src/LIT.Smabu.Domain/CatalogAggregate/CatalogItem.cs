using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public class CatalogItem(CatalogItemId id, CatalogItemNumber number, CatalogId catalogId, CatalogGroupId catalogGroupId, bool isActive,
        string name, string description, Unit unit,
        List<CatalogItemPrice>? prices,
        List<CustomerCatalogItemPrice>? customerPrices) : Entity<CatalogItemId>
    {
        private readonly List<CatalogItemPrice> _prices = prices ?? [];
        private readonly List<CustomerCatalogItemPrice> _customerPrices = customerPrices ?? [];

        public override CatalogItemId Id { get; } = id;
        public CatalogItemNumber Number { get; private set; } = number;
        public CatalogId CatalogId { get; } = catalogId;
        public CatalogGroupId CatalogGroupId { get; private set; } = catalogGroupId;
        public bool IsActive { get; private set; } = isActive;
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public Unit Unit { get; private set; } = unit;
        public IReadOnlyList<CatalogItemPrice> Prices => _prices;
        public IReadOnlyList<CustomerCatalogItemPrice> CustomerPrices => _customerPrices;

        public static CatalogItem Create(CatalogItemId id, CatalogItemNumber number,
            CatalogId catalogId, CatalogGroupId catalogGroupId,
            string name, string description, Unit unit)
        {
            var defaultPrices = new List<CatalogItemPrice>()
            {
                CatalogItemPrice.Create(0)
            };
            return new CatalogItem(id, number, catalogId, catalogGroupId, true, name, description, unit, defaultPrices, null);
        }

        public CatalogItemPrice GetCurrentPrice()
        {
            return Prices
                .Where(p => p.CheckIsValidToday())
                .OrderByDescending(p => p.ValidFrom)
                .FirstOrDefault(CatalogItemPrice.Empty)!;
        }

        public CatalogItemPrice GetCurrentPrice(CustomerId customerId)
        {
            return CustomerPrices?.FirstOrDefault(x => x.CustomerId == customerId) ?? GetCurrentPrice();
        }

        public Result Update(string name, string description, bool isActive, Unit unit)
        {
            List<ErrorDetail> validationErrors = Validate(name, unit);
            if (validationErrors.Count != 0)
            {
                return Result.Failure(validationErrors);
            }

            IsActive = isActive;
            Name = name;
            Description = description;
            Unit = unit;

            return Result.Success();
        }

        public Result UpdatePrices(CatalogItemPrice[] prices, CustomerCatalogItemPrice[] customerPrices)
        {
            if (prices?.Length == 0 || prices?.Where(x => x.CheckIsValidToday()).Count() == 0)
            {
                return CatalogErrors.NoValidPrice;
            }

            _prices.Clear();
            _prices.AddRange(prices!.OrderByDescending(x => x.ValidFrom));

            _customerPrices.Clear();
            _customerPrices.AddRange(customerPrices.DistinctBy(x => x.CustomerId).Where(x => x.Price >= 0));

            return Result.Success();
        }

        private static List<ErrorDetail> Validate(string name, Unit unit)
        {
            var validationErrors = new List<ErrorDetail>();
            if (string.IsNullOrEmpty(name))
            {
                validationErrors.Add(CatalogErrors.NameEmpty);
            }
            if (unit == null)
            {
                validationErrors.Add(CatalogErrors.UnitEmpty);
            }

            return validationErrors;
        }
    }
}
