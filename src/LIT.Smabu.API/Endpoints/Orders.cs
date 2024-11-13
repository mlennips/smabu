using MediatR;
using LIT.Smabu.Domain.OrderAggregate;
using LIT.Smabu.UseCases.Orders;
using LIT.Smabu.Domain.Base;
using static LIT.Smabu.UseCases.Orders.CreateOrder;
using static LIT.Smabu.UseCases.Orders.ListOrders;
using static LIT.Smabu.UseCases.Orders.GetOrder;
using static LIT.Smabu.UseCases.Orders.UpdateOrder;
using static LIT.Smabu.UseCases.Orders.DeleteOrder;
using static LIT.Smabu.UseCases.Orders.GetOrderReferences;
using static LIT.Smabu.UseCases.Orders.UpdateReferencesToOrder;

namespace LIT.Smabu.API.Endpoints
{
    public static class Orders
    {
        public static void RegisterOrdersEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder api = routes.MapGroup("/orders")
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
                .Produces<GetOrderReferencesResponse>();

            api.MapPut("/{orderId}/references", async (IMediator mediator, Guid orderId, UpdateReferencesToOrderCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }
    }
}
