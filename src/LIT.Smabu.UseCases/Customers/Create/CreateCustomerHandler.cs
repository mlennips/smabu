using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Customers.Create
{
    public class CreateCustomerHandler(IAggregateStore store, BusinessNumberService businessNumberService) : ICommandHandler<CreateCustomerCommand, CustomerId>
    {
        public async Task<Result<CustomerId>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var number = await businessNumberService.CreateCustomerNumberAsync();
            request.Number = number;
            var customer = Customer.Create(request.CustomerId, request.Number, request.Name, "");
            await store.CreateAsync(customer);
            return customer.Id;
        }
    }
}
