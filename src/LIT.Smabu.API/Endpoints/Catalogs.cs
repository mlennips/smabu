using MediatR;
using LIT.Smabu.UseCases.Catalogs.Get;
using LIT.Smabu.UseCases.Catalogs;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.UseCases.Catalogs.GetItem;
using LIT.Smabu.UseCases.Catalogs.UpdateItem;
using LIT.Smabu.UseCases.Catalogs.Update;
using LIT.Smabu.UseCases.Catalogs.Delete;
using LIT.Smabu.UseCases.Catalogs.RemoveItem;
using LIT.Smabu.UseCases.Catalogs.GetGroup;
using LIT.Smabu.UseCases.Catalogs.UpdateGroup;
using LIT.Smabu.UseCases.Catalogs.RemoveGroup;
using LIT.Smabu.UseCases.Catalogs.AddGroup;
using LIT.Smabu.UseCases.Catalogs.AddItem;
using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.API.Endpoints
{
    public static class Catalogs
    {
        public static void RegisterCatalogsEndpoints(this IEndpointRouteBuilder routes)
        {
            var api = routes.MapGroup("/catalogs")
                .WithTags(["Catalogs"])
                .RequireAuthorization();

            MapCatalogs(api);
            MapCatalogGroups(api);
            MapCatalogItems(api);
        }

        private static void MapCatalogs(RouteGroupBuilder api)
        {
            api.MapGet("/default", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new GetCatalogQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<CatalogDTO>();

            api.MapPut("/{catalogId}", async (IMediator mediator, Guid catalogId, UpdateCatalogCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces<CustomerId>();

            api.MapDelete("/{catalogId}", async (IMediator mediator, Guid catalogId) =>
                await mediator.SendAndMatchAsync(new DeleteCatalogCommand(new(catalogId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }

        private static void MapCatalogGroups(RouteGroupBuilder api)
        {
            api.MapGet("/{catalogId}/groups/{catalogGroupId}", async (IMediator mediator, Guid catalogId, Guid catalogGroupId) =>
                await mediator.SendAndMatchAsync(new GetCatalogGroupQuery(new(catalogGroupId), new(catalogId)),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<CatalogDTO>();

            api.MapPost("/{catalogId}/groups", async (IMediator mediator, Guid catalogId, AddCatalogGroupCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces<CustomerId>();

            api.MapPut("/{catalogId}/groups/{catalogGroupId}", async (IMediator mediator, Guid catalogId, Guid catalogGroupId,
                UpdateCatalogGroupCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces<CustomerId>();

            api.MapDelete("/{catalogId}/groups/{catalogGroupId}", async (IMediator mediator, Guid catalogId, Guid catalogGroupId) =>
                await mediator.SendAndMatchAsync(new RemoveCatalogGroupCommand(new(catalogGroupId), new(catalogId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }

        private static void MapCatalogItems(RouteGroupBuilder api)
        {
            api.MapGet("/{catalogId}/items/{catalogItemId}", async (IMediator mediator, Guid catalogId, Guid catalogItemId) =>
                await mediator.SendAndMatchAsync(new GetCatalogItemQuery(new(catalogItemId), new(catalogId)),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<CatalogDTO>();

            api.MapPost("/{catalogId}/items", async (IMediator mediator, Guid catalogId, AddCatalogItemCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces<CustomerId>();

            api.MapPut("/{catalogId}/items/{catalogItemId}", async (IMediator mediator, Guid catalogId, Guid catalogItemId,
                UpdateCatalogItemCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces<CustomerId>();

            api.MapDelete("/{catalogId}/items/{catalogItemId}", async (IMediator mediator, Guid catalogId, Guid catalogItemId) =>
                await mediator.SendAndMatchAsync(new RemoveCatalogItemCommand(new(catalogId), new(catalogItemId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }
    }
}
