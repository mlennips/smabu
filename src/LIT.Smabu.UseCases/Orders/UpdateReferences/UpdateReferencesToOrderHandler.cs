using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate.Services;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Orders.UpdateReferences
{
    public class UpdateReferencesToOrderHandler(UpdateReferencesService updateReferencesService) : ICommandHandler<UpdateReferencesToOrderCommand>
    {
        public async Task<Result> Handle(UpdateReferencesToOrderCommand request, CancellationToken cancellationToken)
        {
            var result = await updateReferencesService.StartAsync(request.OrderId, request.References);
            return result;
        }
    }
}
