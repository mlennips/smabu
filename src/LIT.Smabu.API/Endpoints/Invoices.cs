using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.UseCases.Invoices;
using MediatR;
using static LIT.Smabu.UseCases.Invoices.AddInvoiceItem;
using static LIT.Smabu.UseCases.Invoices.CreateInvoice;
using static LIT.Smabu.UseCases.Invoices.DeleteInvoice;
using static LIT.Smabu.UseCases.Invoices.GetInvoice;
using static LIT.Smabu.UseCases.Invoices.GetInvoiceReport;
using static LIT.Smabu.UseCases.Invoices.ListInvoices;
using static LIT.Smabu.UseCases.Invoices.MoveInvoiceItemDown;
using static LIT.Smabu.UseCases.Invoices.MoveInvoiceItemUp;
using static LIT.Smabu.UseCases.Invoices.ReleaseInvoice;
using static LIT.Smabu.UseCases.Invoices.RemoveInvoiceItem;
using static LIT.Smabu.UseCases.Invoices.UpdateInvoice;
using static LIT.Smabu.UseCases.Invoices.UpdateInvoiceItem;
using static LIT.Smabu.UseCases.Invoices.WithdrawReleaseInvoice;

namespace LIT.Smabu.API.Endpoints
{
    public static class Invoices
    {
        public static void RegisterInvoicesEndpoints(this IEndpointRouteBuilder routes)
        {
            var api = routes.MapGroup("/invoices")
                .WithTags(["Invoices"])
                .RequireAuthorization();

            RegisterInvoice(api);
            RegisterInvoiceItem(api);
        }

        private static void RegisterInvoice(RouteGroupBuilder api)
        {
            api.MapPost("/", async (IMediator mediator, CreateInvoiceCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<InvoiceId>();

            api.MapGet("/", async (IMediator mediator, Guid? customerId) =>
                await mediator.SendAndMatchAsync(new ListInvoicesQuery()
                {
                    CustomerId = customerId.HasValue ? new(customerId.Value) : null
                },
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<InvoiceDTO[]>();

            api.MapGet("/{invoiceId}", async (IMediator mediator, Guid invoiceId, bool withItems = false) =>
                await mediator.SendAndMatchAsync(new GetInvoiceQuery(new(invoiceId)) { WithItems = withItems },
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<InvoiceDTO[]>();

            api.MapGet("/{invoiceId}/report", async (IMediator mediator, Guid invoiceId) =>
               await mediator.SendAndMatchAsync(new GetInvoiceReportQuery(new(invoiceId)),
                    onSuccess: (report) =>
                    {
                        var pdf = report.GeneratePdf();
                        return Results.File(pdf, "application/pdf");
                    },
                    onFailure: Results.BadRequest))
            .Produces<IResult>()
            .Produces<ErrorDetail>(400);

            api.MapPut("/{invoiceId}", async (IMediator mediator, Guid invoiceId, UpdateInvoiceCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<InvoiceId>()
                .Produces<ErrorDetail>(400);

            api.MapPut("/{invoiceId}/release", async (IMediator mediator, Guid invoiceId, ReleaseInvoiceCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);

            api.MapPut("/{invoiceId}/withdrawrelease", async (IMediator mediator, Guid invoiceId, WithdrawReleaseInvoiceCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);

            api.MapDelete("/{invoiceId}", async (IMediator mediator, Guid invoiceId) =>
                await mediator.SendAndMatchAsync(new DeleteInvoiceCommand(new(invoiceId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }

        private static void RegisterInvoiceItem(RouteGroupBuilder api)
        {
            api.MapPost("/{invoiceId}/items", async (IMediator mediator, Guid invoiceId, AddInvoiceItemCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<InvoiceItemId>();

            api.MapPut("/{invoiceId}/items/{invoiceItemId}", async (IMediator mediator, Guid invoiceId, Guid invoiceItemId,
                UpdateInvoiceItemCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<InvoiceItemId>();

            api.MapPut("/{invoiceId}/items/{invoiceItemId}/movedown", async (IMediator mediator, Guid invoiceId, Guid invoiceItemId) =>
                await mediator.SendAndMatchAsync(new MoveInvoiceItemDownCommand(new(invoiceItemId), new(invoiceId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);

            api.MapPut("/{invoiceId}/items/{invoiceItemId}/moveup", async (IMediator mediator, Guid invoiceId, Guid invoiceItemId) =>
                await mediator.SendAndMatchAsync(new MoveInvoiceItemUpCommand(new(invoiceItemId), new(invoiceId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);

            api.MapDelete("/{invoiceId}/items/{invoiceItemId}", async (IMediator mediator, Guid invoiceId, Guid invoiceItemId) =>
                await mediator.SendAndMatchAsync(new RemoveInvoiceItemCommand(new(invoiceItemId), new(invoiceId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }
    }
}