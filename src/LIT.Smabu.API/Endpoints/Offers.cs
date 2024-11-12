using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.UseCases.Offers.AddOfferItem;
using LIT.Smabu.UseCases.Offers.Create;
using LIT.Smabu.UseCases.Offers.Delete;
using LIT.Smabu.UseCases.Offers.Get;
using LIT.Smabu.UseCases.Offers.List;
using LIT.Smabu.UseCases.Offers.RemoveOfferItem;
using LIT.Smabu.UseCases.Offers.Update;
using LIT.Smabu.UseCases.Offers.UpdateOfferItem;
using MediatR;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Offers;
using LIT.Smabu.UseCases.Offers.CreateReport;
using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.API.Endpoints
{
    public static class Offers
    {
        public static void RegisterOffersEndpoints(this IEndpointRouteBuilder routes)
        {
            var api = routes.MapGroup("/offers")
                .WithTags(["Offers"])
                .RequireAuthorization();

            RegisterOffer(api);
            RegisterOfferItem(api);
        }

        private static void RegisterOffer(RouteGroupBuilder api)
        {
            api.MapPost("/", async (IMediator mediator, CreateOfferCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<OfferId>();

            api.MapGet("/", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new ListOffersQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<OfferDTO[]>();

            api.MapGet("/{offerId}", async (IMediator mediator, Guid offerId, bool withItems = false) =>
                await mediator.SendAndMatchAsync(new GetOfferQuery(new(offerId)) { WithItems = withItems },
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<OfferDTO[]>();

            api.MapGet("/{offerId}/report", async (IMediator mediator, Guid offerId) =>
               await mediator.SendAndMatchAsync(new GetOfferReportQuery(new(offerId)),
                    onSuccess: (report) => {
                        var pdf = report.GeneratePdf();
                        return Results.File(pdf, "application/pdf");
                    },
                    onFailure: Results.BadRequest))
            .Produces<IResult>()
            .Produces<ErrorDetail>(400);

            api.MapPut("/{offerId}", async (IMediator mediator, Guid offerId, UpdateOfferCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<OfferId>()
                .Produces<ErrorDetail>(400);

            api.MapDelete("/{offerId}", async (IMediator mediator, Guid offerId) =>
                await mediator.SendAndMatchAsync(new DeleteOfferCommand(new(offerId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }

        private static void RegisterOfferItem(RouteGroupBuilder api)
        {
            api.MapPost("/{offerId}/items", async (IMediator mediator, Guid offerId, AddOfferItemCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<InvoiceItemId>();

            api.MapPut("/{offerId}/items/{itemId}", async (IMediator mediator, Guid offerId, Guid itemId, UpdateOfferItemCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces<InvoiceItemId>();

            api.MapDelete("/{offerId}/items/{itemId}", async (IMediator mediator, Guid offerId, Guid itemId) =>
                await mediator.SendAndMatchAsync(new RemoveOfferItemCommand(new(itemId), new(offerId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }
    }
}
