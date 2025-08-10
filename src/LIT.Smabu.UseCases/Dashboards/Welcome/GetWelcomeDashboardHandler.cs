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

        public class GetWelcomeDashboardHandler(SalesStatisticsService salesStatisticsService,
            IAggregateCache cache) : IQueryHandler<GetWelcomeDashboardQuery, GetWelcomeDashboardReadModel>
        {
            public async Task<Result<GetWelcomeDashboardReadModel>> Handle(GetWelcomeDashboardQuery request, CancellationToken cancellationToken)
            {
                GetWelcomeDashboardReadModel readModel = new()
                {
                    Version = DateTime.Now,
                    ThisYear = DateTime.Now.Year,
                    LastYear = DateTime.Now.Year - 1,
                    Currency = Currency.EUR,
                };

                await Task.WhenAll(
                    SetCountsAsync(cache, readModel),
                    SetSalesVolumesAsync(readModel)
                );

                return Result.Success(readModel);
            }

            private static async Task SetCountsAsync(IAggregateCache cache, GetWelcomeDashboardReadModel readModel)
            {
                readModel.InvoiceCount = await cache.CountAsync<Invoice>();
                readModel.OfferCount = await cache.CountAsync<Offer>();
                readModel.CustomerCount = await cache.CountAsync<Customer>();
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
