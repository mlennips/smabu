using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.CustomerAggregate.Services;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Customers
{
    public static class DeleteCustomer
    {
        public record DeleteCustomerCommand(CustomerId CustomerId) : ICommand;

        public class DeleteCustomerHandler(DeleteCustomerService deleteCustomerService) : ICommandHandler<DeleteCustomerCommand>
        {
            public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
            {
                var result = await deleteCustomerService.DeleteAsync(request.CustomerId);
                if (result.IsFailure)
                {
                    return result.Error;
                }
                return Result.Success();
            }
        }
    }
}
