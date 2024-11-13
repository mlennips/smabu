using MediatR;
using LIT.Smabu.UseCases.Catalogs;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.Base;
using static LIT.Smabu.UseCases.Catalogs.GetCatalog;
using static LIT.Smabu.UseCases.Catalogs.DeleteCatalog;
using static LIT.Smabu.UseCases.Catalogs.GetCatalogGroup;
using static LIT.Smabu.UseCases.Catalogs.AddCatalogGroup;
using static LIT.Smabu.UseCases.Catalogs.RemoveCatalogGroup;
using static LIT.Smabu.UseCases.Catalogs.GetCatalogItem;
using static LIT.Smabu.UseCases.Catalogs.AddCatalogItem;
using static LIT.Smabu.UseCases.Catalogs.UpdateCatalogItem;
using static LIT.Smabu.UseCases.Catalogs.RemoveCatalogItem;
using static LIT.Smabu.UseCases.Catalogs.UpdateCatalog;
using static LIT.Smabu.UseCases.Catalogs.UpdateCatalogGroup;

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
