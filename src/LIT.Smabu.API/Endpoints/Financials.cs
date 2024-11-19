using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Financial;
using MediatR;
using static LIT.Smabu.UseCases.Financial.CompleteAnnualFinancialStatement;
using static LIT.Smabu.UseCases.Financial.CreateAnnualFinancialStatement;
using static LIT.Smabu.UseCases.Financial.DeleteAnnualFinancialStatement;
using static LIT.Smabu.UseCases.Financial.GetAnnualFinancialStatement;
using static LIT.Smabu.UseCases.Financial.ImportAnnualFinancialStatementTransactions;
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

            api.MapGet("/annualstatements/", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new ListAnnualFinancialStatementsQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<AnnualFinancialStatementDTO[]>();

            api.MapGet("/annualstatements/{annualFinancialStatementId}", async (IMediator mediator, Guid annualFinancialStatementId) =>
                await mediator.SendAndMatchAsync(new GetAnnualFinancialStatementQuery(new(annualFinancialStatementId)),
                    onSuccess: Results.Ok,
                    onFailure: Results.NotFound))
                .Produces<AnnualFinancialStatementDTO>()
                .Produces(StatusCodes.Status404NotFound);

            api.MapPost("/annualstatements/", async (IMediator mediator, CreateAnnualFinancialStatementCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Created,
                    onFailure: Results.BadRequest))
                .Produces<AnnualFinancialStatementId>(StatusCodes.Status201Created);

            api.MapPut("/annualstatements/{annualFinancialStatementId}", async (IMediator mediator, Guid annualFinancialStatementId,
                UpdateAnnualFinancialStatementCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.NoContent,
                    onFailure: Results.NotFound))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

            api.MapPut("/annualstatements/{annualFinancialStatementId}/importtransactions", async (IMediator mediator, Guid annualFinancialStatementId) =>
                await mediator.SendAndMatchAsync(new ImportAnnualFinancialStatementTransactionsCommand(new(annualFinancialStatementId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.NotFound))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            api.MapPut("/annualstatements/{annualFinancialStatementId}/complete", async (IMediator mediator, Guid annualFinancialStatementId) =>
                await mediator.SendAndMatchAsync(new CompleteAnnualFinancialStatementCommand(new(annualFinancialStatementId)),
                    onSuccess: Results.NoContent,
                    onFailure: Results.NotFound))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

            api.MapDelete("/annualstatements/{annualFinancialStatementId}", async (IMediator mediator, Guid annualFinancialStatementId) =>
                await mediator.SendAndMatchAsync(new DeleteAnnualFinancialStatementCommand(new(annualFinancialStatementId)),
                    onSuccess: () => Results.NoContent(),
                    onFailure: Results.NotFound))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);
        }
    }
}
