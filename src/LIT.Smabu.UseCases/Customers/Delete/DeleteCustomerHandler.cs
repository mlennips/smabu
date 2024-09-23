﻿using LIT.Smabu.Domain.CustomerAggregate.Services;
using LIT.Smabu.Domain.SeedWork;
using LIT.Smabu.UseCases.SeedWork;

namespace LIT.Smabu.UseCases.Customers.Delete
{
    public class DeleteCustomerHandler(DeleteCustomerService deleteCustomerService) : ICommandHandler<DeleteCustomerCommand>
    {
        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            return await deleteCustomerService.DeleteAsync(request.Id);
        }
    }
}
