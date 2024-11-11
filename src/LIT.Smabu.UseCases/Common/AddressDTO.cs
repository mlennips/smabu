using LIT.Smabu.Domain.Common;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Common
{
    public record AddressDTO(string Name1, string Name2, string Street, string HouseNumber, string PostalCode, string City, string Country) : IDTO
    {
        public string DisplayName => $"{Name1}, {Street}, {PostalCode} {City} - {Country}";

        public static AddressDTO Create(Address address)
        {
            return new AddressDTO(address.Name1, address.Name2, address.Street, address.HouseNumber, address.PostalCode, address.City, address.Country);
        }
    }
}
