using MediatR;
using LIT.Smabu.UseCases.Orders.Create;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.UseCases.Orders;
using LIT.Smabu.UseCases.Orders.List;
using LIT.Smabu.UseCases.Orders.Get;
using LIT.Smabu.UseCases.Orders.Update;
using LIT.Smabu.UseCases.Orders.Delete;
using LIT.Smabu.UseCases.Orders.UpdateReferences;
using LIT.Smabu.UseCases.Orders.GetReferences;
using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.API.Endpoints
{
    public static class Orders
    {
        public static void RegisterOrdersEndpoints(this IEndpointRouteBuilder routes)
        {
            var api = routes.MapGroup("/orders")
                .WithTags(["Orders"])
                .RequireAuthorization();

            RegisterOrder(api);
        }

        private static void RegisterOrder(RouteGroupBuilder api)
        {
            api.MapPost("/", async (IMediator mediator, CreateOrderCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<OrderId>();

            api.MapGet("/", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new ListOrdersQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<OrderDTO[]>();

            api.MapGet("/{orderId}", async (IMediator mediator, Guid orderId, bool withItems = false) =>
                await mediator.SendAndMatchAsync(new GetOrderQuery(new(orderId)),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<OrderDTO[]>();

            api.MapPut("/{orderId}", async (IMediator mediator, Guid orderId, UpdateOrderCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<OrderId>()
                .Produces<ErrorDetail>(400);

            api.MapDelete("/{orderId}", async (IMediator mediator, Guid orderId) =>
                await mediator.SendAndMatchAsync(new DeleteOrderCommand(new(orderId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);

            api.MapGet("/{orderId}/references", async (IMediator mediator, Guid orderId) =>
                await mediator.SendAndMatchAsync(new GetOrderReferencesQuery(new(orderId)),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<GetOrderReferencesReadModel>();

            api.MapPut("/{orderId}/references", async (IMediator mediator, Guid orderId, UpdateReferencesToOrderCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }
    }
}
