using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using static LIT.Smabu.UseCases.Dashboards.Sales.GetSalesDashboardReadModel;

namespace LIT.Smabu.UseCases.Dashboards.Sales
{
    public static class GetSalesDashboard
    {
        public record GetSalesDashboardQuery : IQuery<GetSalesDashboardReadModel>;

        public class GetSalesDashboardHandler(SalesStatisticsService salesStatisticsService, IAggregateStore store,
            IMemoryCache cache) : IQueryHandler<GetSalesDashboardQuery, GetSalesDashboardReadModel>
        {
            private const string CACHE_KEY = "SalesDashboardResult";

            public async Task<Result<GetSalesDashboardReadModel>> Handle(GetSalesDashboardQuery request, CancellationToken cancellationToken)
            {
                if (cache.TryGetValue(CACHE_KEY, out GetSalesDashboardReadModel? readModel))
                {
                    return Result<GetSalesDashboardReadModel>.Success(readModel!);
                }

                readModel = new GetSalesDashboardReadModel
                {
                    Version = DateTime.Now,
                    ThisYear = DateTime.Now.Year,
                    LastYear = DateTime.Now.Year - 1,
                    Currency = Currency.EUR
                };

                var customers = await store.GetAllAsync<Customer>();

                var salesTasks = new Task[]
                {
                SetSalesInformationsAsync(readModel),
                SetSalesByYearDatasetAsync(readModel, customers),
                SetSalesByCustomerAsync(readModel, customers)
                };
                await Task.WhenAll(salesTasks);

                cache.Set(CACHE_KEY, readModel, TimeSpan.FromMinutes(5));
                return Result<GetSalesDashboardReadModel>.Success(readModel);
            }

            private async Task SetSalesInformationsAsync(GetSalesDashboardReadModel result)
            {
                result.SalesThisYear = await salesStatisticsService.CalculateSalesForYearAsync(result.ThisYear);
                result.SalesLastYear = await salesStatisticsService.CalculateSalesForYearAsync(result.LastYear);
                result.SalesLast12Month = await salesStatisticsService.CalculateSalesForLastMonthsAsync(12);
                result.SalesLast24Month = await salesStatisticsService.CalculateSalesForLastMonthsAsync(24);
                result.SalesLast36Month = await salesStatisticsService.CalculateSalesForLastMonthsAsync(36);
                result.TotalSales = await salesStatisticsService.CalculateTotalSalesAsync();
                result.Top3InvoicesEver = (await salesStatisticsService.GetHighestInvoicesAsync(3))
                    .Select(x => new SalesAmountItem(x.Number.ToString(), x.Id.ToString(), x.Amount)).ToList();
                result.Top3InvoicesLast12Month = (await salesStatisticsService.GetHighestInvoicesAsync(3, 12))
                    .Select(x => new SalesAmountItem(x.Number.ToString(), x.Id.ToString(), x.Amount)).ToList();
                result.InvoiceCount = await store.CountAsync<Invoice>();
                result.CustomerCount = await store.CountAsync<Customer>();
                result.OrderCount = 0;
            }

            private async Task SetSalesByYearDatasetAsync(GetSalesDashboardReadModel result, IReadOnlyList<Customer> customers)
            {
                var salesByYear = await salesStatisticsService.GetSalesByYearAsync();
                var startYear = salesByYear.Items.Min(x => x.Year);
                var endYear = DateTime.Now.Year;

                var totalSerie = new Dataset.Serie("Total", "total");
                var yearSerie = new Dataset.Serie("Jahr", "year");
                var customerSeries = customers.Select(x => new Dataset.Serie(x.CorporateDesign.ShortName, x.Id.ToString(), "customer")).ToList();
                var series = new List<Dataset.Serie>
            {
                totalSerie,
                yearSerie
            };
                series.AddRange(customerSeries);
                result.SalesByYear = new Dataset
                {
                    Series = series
                };

                decimal totalAmount = 0;
                foreach (var year in Enumerable.Range(startYear, endYear - startYear + 1))
                {
                    result.SalesByYear.ValueLabels.Add(year.ToString("0000", CultureInfo.InvariantCulture));
                    var yearItem = salesByYear.Items.FirstOrDefault(x => x.Year == year);
                    totalAmount += yearItem?.Amount ?? 0;
                    totalSerie.Values.Add(totalAmount);
                    yearSerie.Values.Add(yearItem?.Amount ?? 0);
                    foreach (var customerSerie in customerSeries)
                    {
                        var customerData = yearItem?.Customers.FirstOrDefault(x => x.CustomerId.ToString() == customerSerie.Key);
                        customerSerie.Values.Add(customerData?.TotalAmount ?? 0);
                    }
                }
            }

            private async Task SetSalesByCustomerAsync(GetSalesDashboardReadModel result, IReadOnlyList<Customer> customers)
            {
                var salesByCustomer = await salesStatisticsService.GetSalesByCustomerAsync();
                result.SalesByCustomer = salesByCustomer
                    .Select(x => new SalesAmountItem(
                        customers.Single(c => c.Id == x.Key).CorporateDesign.ShortName,
                        customers.Single(c => c.Id == x.Key).Id.ToString(),
                        x.Value))
                    .ToList();
            }
        }
    }
}
