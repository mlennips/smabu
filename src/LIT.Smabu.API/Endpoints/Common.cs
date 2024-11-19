using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.Domain.PaymentAggregate;

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
                .Produces<Unit[]>();

            api.MapGet("/paymentmethods", () => PaymentMethod.GetAll())
                .Produces<PaymentMethod[]>();

            api.MapGet("/paymentconditions", () => PaymentCondition.GetAll())
                .Produces<PaymentCondition[]>();

            api.MapGet("/financialcategories/incomes", () => FinancialCategory.GetAllIncomes())
                .Produces<FinancialCategory[]>();

            api.MapGet("/financialcategories/expenditures", () => FinancialCategory.GetAllExpenditures())
                .Produces<FinancialCategory[]>();
        }
    }
}
