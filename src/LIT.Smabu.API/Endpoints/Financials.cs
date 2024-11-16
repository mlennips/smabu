using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Financial;
using MediatR;
using static LIT.Smabu.UseCases.Financial.CreateAnnualFinancialStatement;
using static LIT.Smabu.UseCases.Financial.DeleteAnnualFinancialStatement;
using static LIT.Smabu.UseCases.Financial.GetAnnualFinancialStatement;
using static LIT.Smabu.UseCases.Financial.ListAnnualFinancialStatements;
using static LIT.Smabu.UseCases.Financial.UpdateAnnualFinancialStatement;

namespace LIT.Smabu.API.Endpoints
{
    public static class Financials
    {
        public static void RegisterFinancialEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder api = routes.MapGroup("/financial")
                .WithTags(["Financial"])
                .RequireAuthorization();

            api.MapGet("/", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new ListAnnualFinancialStatementsQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<AnnualFinancialStatementDTO[]>();

            api.MapGet("/{annualFinancialStatementId}", async (IMediator mediator, Guid annualFinancialStatementId) =>
                await mediator.SendAndMatchAsync(new GetAnnualFinancialStatementQuery(new(annualFinancialStatementId)),
                    onSuccess: Results.Ok,
                    onFailure: Results.NotFound))
                .Produces<AnnualFinancialStatementDTO>()
                .Produces(StatusCodes.Status404NotFound);

            api.MapPost("/", async (IMediator mediator, CreateAnnualFinancialStatementCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Created,
                    onFailure: Results.BadRequest))
                .Produces<AnnualFinancialStatementId>(StatusCodes.Status201Created);

            api.MapPut("/{annualFinancialStatementId}", async (IMediator mediator, Guid annualFinancialStatementId,
                UpdateAnnualFinancialStatementCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.NoContent,
                    onFailure: Results.NotFound))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

            api.MapDelete("/{annualFinancialStatementId}", async (IMediator mediator, Guid annualFinancialStatementId) =>
                await mediator.SendAndMatchAsync(new DeleteAnnualFinancialStatementCommand(new(annualFinancialStatementId)),
                    onSuccess: () => Results.NoContent(),
                    onFailure: Results.NotFound))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);
        }
    }
}
