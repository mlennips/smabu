using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;
using Microsoft.Extensions.Caching.Memory;

namespace LIT.Smabu.UseCases.Dashboards.Welcome
{
    public static class GetWelcomeDashboard
    {
        public record GetWelcomeDashboardQuery : IQuery<GetWelcomeDashboardReadModel>;

        public class GetWelcomeDashboardHandler(SalesStatisticsService salesStatisticsService, IAggregateStore store,
            IMemoryCache cache) : IQueryHandler<GetWelcomeDashboardQuery, GetWelcomeDashboardReadModel>
        {
            private const string CACHE_KEY = "WelcomeDashboardResult";

            public async Task<Result<GetWelcomeDashboardReadModel>> Handle(GetWelcomeDashboardQuery request, CancellationToken cancellationToken)
            {

                if (cache.TryGetValue(CACHE_KEY, out GetWelcomeDashboardReadModel? readModel))
                {
                    return Result<GetWelcomeDashboardReadModel>.Success(readModel!);
                }

                readModel = new()
                {
                    Version = DateTime.Now,
                    ThisYear = DateTime.Now.Year,
                    LastYear = DateTime.Now.Year - 1,
                    Currency = Currency.EUR,
                };

                await Task.WhenAll(
                    SetCountsAsync(store, readModel),
                    SetSalesVolumesAsync(readModel)
                );

                cache.Set(CACHE_KEY, readModel, TimeSpan.FromMinutes(5));
                return Result<GetWelcomeDashboardReadModel>.Success(readModel);
            }

            private static async Task SetCountsAsync(IAggregateStore store, GetWelcomeDashboardReadModel readModel)
            {
                readModel.InvoiceCount = await store.CountAsync<Invoice>();
                readModel.OfferCount = await store.CountAsync<Offer>();
                readModel.CustomerCount = await store.CountAsync<Customer>();
            }

            private async Task SetSalesVolumesAsync(GetWelcomeDashboardReadModel result)
            {
                result.SalesVolumeThisYear = await salesStatisticsService.CalculateSalesForYearAsync(result.ThisYear);
                result.SalesVolumeLastYear = await salesStatisticsService.CalculateSalesForYearAsync(result.LastYear);
                result.TotalSalesVolume = await salesStatisticsService.CalculateTotalSalesAsync();
            }
        }
    }
}
