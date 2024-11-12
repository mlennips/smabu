using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Customers
{
    public static class UpdateCustomer
    {
        public record UpdateCustomerCommand(CustomerId CustomerId, string Name, string IndustryBranch,
            Address? MainAddress, Communication? Communication, CorporateDesign? CorporateDesign) : ICommand<CustomerId>;

        public class UpdateCustomerHandler(IAggregateStore store) : ICommandHandler<UpdateCustomerCommand, CustomerId>
        {
            public async Task<Result<CustomerId>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = await store.GetByAsync(request.CustomerId);
                customer.Update(request.Name, request.IndustryBranch ?? "", request.MainAddress, request.Communication, request.CorporateDesign);
                await store.UpdateAsync(customer);
                return customer.Id;
            }
        }
    }
}
