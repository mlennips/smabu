﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.UseCases.Common;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Customers
{
    public record CustomerDTO : IDTO
    {
        public CustomerDTO(CustomerId id, CustomerNumber number, string name, string industryBranch, 
            Currency currency, AddressDTO mainAddress, CommunicationDTO communication, CorporateDesign corporateDesign)
        {
            Id = id;
            Number = number;
            Name = name;
            IndustryBranch = industryBranch;
            Currency = currency;
            MainAddress = mainAddress;
            Communication = communication;
            CorporateDesign = corporateDesign;
        }

        public string DisplayName => $"{Number.Long}/{CorporateDesign.ShortName}";
        public CustomerId Id { get; set; }
        public CustomerNumber Number { get; set; }
        public string Name { get; set; }
        public string IndustryBranch { get; set; }
        public Currency Currency { get; set; }
        public AddressDTO MainAddress { get; set; }
        public CommunicationDTO Communication { get; set; }
        public CorporateDesign CorporateDesign { get; set; }

        public static CustomerDTO Create(Customer customer)
        {
            return new CustomerDTO(customer.Id, customer.Number, customer.Name, customer.IndustryBranch, customer.Currency,
                AddressDTO.From(customer.MainAddress), CommunicationDTO.From(customer.Communication), customer.CorporateDesign);
        }
    }
}
