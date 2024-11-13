using MediatR;
using LIT.Smabu.UseCases.Dashboards.Welcome;
using LIT.Smabu.UseCases.Dashboards.Sales;
using static LIT.Smabu.UseCases.Dashboards.Welcome.GetWelcomeDashboard;
using static LIT.Smabu.UseCases.Dashboards.Sales.GetSalesDashboard;

namespace LIT.Smabu.API.Endpoints
{
    public static class Dashboards
    {
        public static void RegisterDashboardEndpoints(this IEndpointRouteBuilder routes)
        {
            var api = routes.MapGroup("/dashboards")
            .WithTags(["Dashboards"])
            .RequireAuthorization();

            api.MapGet("/welcome", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new GetWelcomeDashboardQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<GetWelcomeDashboardReadModel>();

            api.MapGet("/sales", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new GetSalesDashboardQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<GetSalesDashboardReadModel>();
        }
    }
}
