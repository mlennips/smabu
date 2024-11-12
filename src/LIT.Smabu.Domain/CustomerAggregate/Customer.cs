using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;

namespace LIT.Smabu.Domain.CustomerAggregate
{
    public class Customer(CustomerId id, CustomerNumber number, string name, string industryBranch,
        Currency currency, Address mainAddress, Communication communication, CorporateDesign corporateDesign) 
        : AggregateRoot<CustomerId>, IHasBusinessNumber<CustomerNumber>
    {
        public override CustomerId Id { get; } = id;
        public CustomerNumber Number { get; private set; } = number;
        public string Name { get; private set; } = name;
        public string IndustryBranch { get; private set; } = industryBranch;
        public Currency Currency { get; set; } = currency;
        public Address MainAddress { get; private set; } = mainAddress;
        public Communication Communication { get; private set; } = communication;
        public CorporateDesign CorporateDesign { get; private set; } = corporateDesign;
        
        public static Customer Create(CustomerId id, CustomerNumber number, string name, string industryBranch)
        {
            var corporateDesign = CorporateDesign.CreateDefault(name);
            return new Customer(id, number, name, industryBranch, Currency.EUR,
                new Address(name, "", "", "", "", "", ""), Communication.Empty, corporateDesign);
        }

        public Result Update(string name, string? industryBranch, Address? mainAddress, 
            Communication? communication, CorporateDesign? corporateDesign)
        {
            Name = name;
            IndustryBranch = industryBranch ?? "";
            if (mainAddress != null)
            {
                MainAddress = mainAddress;
            }
            if (communication != null)
            {
                Communication = communication;
            }
            if (corporateDesign != null)
            {
                CorporateDesign = corporateDesign;
            }
            return Result.Success();
        }

        public override Result Delete()
        {
            return base.Delete();
        }
    }
}
