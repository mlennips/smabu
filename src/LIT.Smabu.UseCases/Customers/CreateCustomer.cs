using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Customers
{
    public static class CreateCustomer
    {
        public record CreateCustomerCommand(CustomerId CustomerId, string Name, CustomerNumber? Number) : ICommand<CustomerId>;

        public class CreateCustomerHandler(IAggregateStore store, BusinessNumberService businessNumberService) : ICommandHandler<CreateCustomerCommand, CustomerId>
        {
            public async Task<Result<CustomerId>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var number = await businessNumberService.CreateCustomerNumberAsync();
                var customer = Customer.Create(request.CustomerId, number, request.Name, "");
                await store.CreateAsync(customer);
                return customer.Id;
            }
        }
    }
}
