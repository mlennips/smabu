using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate.Services;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Customers.Delete
{
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
