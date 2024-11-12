using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.Domain.OrderAggregate.Services;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Orders
{
    public static class UpdateReferencesToOrder
    {
        public record UpdateReferencesToOrderCommand(OrderId OrderId, OrderReferences References) : ICommand;

        public class UpdateReferencesToOrderHandler(UpdateReferencesService updateReferencesService) : ICommandHandler<UpdateReferencesToOrderCommand>
        {
            public async Task<Result> Handle(UpdateReferencesToOrderCommand request, CancellationToken cancellationToken)
            {
                var result = await updateReferencesService.StartAsync(request.OrderId, request.References);
                return result;
            }
        }
    }
}
