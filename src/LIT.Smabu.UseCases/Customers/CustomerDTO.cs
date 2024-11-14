using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.UseCases.Common;

namespace LIT.Smabu.UseCases.Customers
{
    public record CustomerDTO : IDTO
    {
        public string DisplayName => $"{Number.DisplayName}/{CorporateDesign.ShortName}";
        public CustomerId Id { get; set; }
        public CustomerNumber Number { get; set; }
        public string Name { get; set; }
        public string IndustryBranch { get; set; }
        public Currency Currency { get; set; }
        public AddressDTO MainAddress { get; set; }
        public CommunicationDTO Communication { get; set; }
        public CorporateDesign CorporateDesign { get; set; }
        public string VatId { get; set; }

        public CustomerDTO(CustomerId id, CustomerNumber number, string name, string industryBranch,
            Currency currency, AddressDTO mainAddress, CommunicationDTO communication, CorporateDesign corporateDesign, string vatId)
        {
            Id = id;
            Number = number;
            Name = name;
            IndustryBranch = industryBranch;
            Currency = currency;
            MainAddress = mainAddress;
            Communication = communication;
            CorporateDesign = corporateDesign;
            VatId = vatId;
        }

        public static CustomerDTO Create(Customer customer)
        {
            return new CustomerDTO(customer.Id, customer.Number, customer.Name, customer.IndustryBranch, customer.Currency,
                AddressDTO.Create(customer.MainAddress), CommunicationDTO.Create(customer.Communication), customer.CorporateDesign, customer.VatId);
        }
    }
}
