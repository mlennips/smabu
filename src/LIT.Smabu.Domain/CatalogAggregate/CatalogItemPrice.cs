using LIT.Smabu.Domain.Common;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public record CatalogItemPrice : IValueObject
    {

        public decimal Price { get; set; }
        public Currency Currency { get; } = Currency.EUR;
        public DateTime ValidFrom { get; set; }

        public CatalogItemPrice(decimal price, DateTime validFrom)
        {
            Price = price;
            ValidFrom = validFrom;
        }

        public static CatalogItemPrice Create(int price)
        {
            return new(price, DateTime.UtcNow.Date);
        }

        public static CatalogItemPrice? Empty => new(0, DateTime.UtcNow.Date);

        public bool CheckIsValidToday()
        {
            return ValidFrom.Date <= DateTime.UtcNow.Date;
        }
    }
}
