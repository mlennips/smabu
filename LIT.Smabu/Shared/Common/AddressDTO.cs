﻿using LIT.Smabu.Shared.Contracts;

namespace LIT.Smabu.Shared.Common
{
    public class AddressDTO : IDTO
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Additional { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
