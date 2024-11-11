using LIT.Smabu.Shared;

namespace LIT.Smabu.Domain.Common
{
    public record Address : IValueObject
    {
        public string Name1 { get; private set; }
        public string Name2 { get; private set; }
        public string Street { get; private set; }
        public string HouseNumber { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }

        public Address(string name1, string name2, string street, string houseNumber, string postalCode, string city, string country)
        {
            Name1 = name1;
            Name2 = name2;
            Street = street;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }
    }
}
