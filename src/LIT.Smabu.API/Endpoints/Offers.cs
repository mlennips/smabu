using LIT.Smabu.Domain.InvoiceAggregate;
using MediatR;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Offers;
using LIT.Smabu.Domain.Base;
using static LIT.Smabu.UseCases.Offers.CreateOffer;
using static LIT.Smabu.UseCases.Offers.ListOffers;
using static LIT.Smabu.UseCases.Offers.GetOffer;
using static LIT.Smabu.UseCases.Offers.GetOfferReport;
using static LIT.Smabu.UseCases.Offers.UpdateOffer;
using static LIT.Smabu.UseCases.Offers.DeleteOffer;
using static LIT.Smabu.UseCases.Offers.AddOfferItem;
using static LIT.Smabu.UseCases.Offers.UpdateOfferItem;
using static LIT.Smabu.UseCases.Offers.RemoveOfferItem;

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
