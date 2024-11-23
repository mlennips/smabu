using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;

namespace LIT.Smabu.API.Endpoints
{
    public static class Common
    {
        public static void RegisterCommonEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder api = routes.MapGroup("/common")
                .WithTags(["Common"]);

            RegisterEnums(api);
        }

        private static void RegisterEnums(RouteGroupBuilder api)
        {
            RouteGroupBuilder enumsApi = api.MapGroup("/enums")
                .WithTags("Enums");

            enumsApi.MapGet("/", () => BuildEnums())
                .Produces<EnumsResponse>();
        }

        private static EnumsResponse BuildEnums()
        {
            return new EnumsResponse
            {
                Currencies = Currency.GetAll(),
                PaymentMethods = PaymentMethod.GetAll(),
                PaymentConditions = PaymentCondition.GetAll(),
                Units = Unit.GetAll(),
                FinancialCategoryIncomes = FinancialCategory.GetAllIncomes(),
                FinancialCategoryExpenditures = FinancialCategory.GetAllExpenditures(),
                TaxRates = TaxRate.GetAll()
            };
        }

        public record EnumsResponse
        {
            public required Currency[] Currencies { get; init; }
            public required PaymentMethod[] PaymentMethods { get; init; }
            public required PaymentCondition[] PaymentConditions { get; init; }
            public required Unit[] Units { get; init; }
            public required FinancialCategory[] FinancialCategoryIncomes { get; init; }
            public required FinancialCategory[] FinancialCategoryExpenditures { get; init; }
            public required TaxRate[] TaxRates { get; init; }
        }
    }
}
