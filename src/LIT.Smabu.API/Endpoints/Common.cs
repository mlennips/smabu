using LIT.Smabu.Domain.Common;

namespace LIT.Smabu.API.Endpoints
{
    public static class Common
    {
        public static void RegisterCommonEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder api = routes.MapGroup("/common")
                .WithTags(["Common"]);

            api.MapGet("/currencies", () => Currency.GetAll())
                .Produces<Currency[]>();

            api.MapGet("/taxrates", () => TaxRate.GetAll())
                .Produces<TaxRate[]>();

            api.MapGet("/units", () => Unit.GetAll())
                .Produces<string[]>();
        }
    }
}
