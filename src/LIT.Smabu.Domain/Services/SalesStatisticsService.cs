using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.Services
{
    public class SalesStatisticsService(IAggregateStore store)
    {
        private IReadOnlyList<Invoice>? invoices;

        public async Task<decimal> CalculateSalesForYearAsync(int fiscalYear)
        {
            return (await GetInvoicesAsync())
                .Where(x => x.FiscalYear == fiscalYear)
                .Sum(x => x.Amount);
        }

        public async Task<decimal> CalculateSalesForLastMonthsAsync(uint monthsBack)
        {
            return (await GetInvoicesAsync())
                .Where(x => x.Meta!.CreatedAt >= DateTime.Now.AddMonths((int)monthsBack * -1))
                .Sum(x => x.Amount);
        }

        public async Task<decimal> CalculateTotalSalesAsync()
        {
            return (await GetInvoicesAsync())
                .Sum(x => x.Amount);
        }

        public async Task<decimal> CalculateSalesForCustomerAsync(CustomerId customerId)
        {
            return (await GetInvoicesAsync())
                .Where(x => x.CustomerId == customerId)
                .Sum(x => x.Amount);
        }

        public async Task<Invoice[]> GetHighestInvoicesAsync(int limit = 3, uint monthsBack = 0)
        {
            return (await GetInvoicesAsync())
                .Where(x => monthsBack == 0 || x.Meta!.CreatedAt >= DateTime.Now.AddMonths((int)monthsBack * -1))
                .OrderByDescending(x => x.Amount)
                .Take(limit)
                .ToArray();
        }

        public async Task<GetSalesByYear> GetSalesByYearAsync()
        {
            var invoices = await GetInvoicesAsync();

            GetSalesByYear result = new()
            {
                Items = [.. invoices.GroupBy(x => x.FiscalYear)
                    .Select(x => new GetSalesByYear.Item
                    {
                        Year = x.Key,
                        Amount = x.Sum(i => i.Amount),
                        Customers = x.GroupBy(c => c.CustomerId)
                            .Select(c => new GetSalesByYear.Item.Customer
                            {
                                TotalAmount = c.Sum(ci => ci.Amount),
                                CustomerId = c.Key,
                            }).ToArray()
                    })
                    .OrderBy(x => x.Year)]
            };
            return result;
        }

        public async Task<Dictionary<CustomerId, decimal>> GetSalesByCustomerAsync()
        {
            var invoices = await GetInvoicesAsync();

            var result = invoices
                .GroupBy(invoice => invoice.CustomerId)
                .Select(x => new { x.Key, Value = x.Sum(invoice => invoice.Amount) })
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
            return result;
        }

        private async Task<IReadOnlyList<Invoice>> GetInvoicesAsync() => invoices ??= await store.GetAllAsync<Invoice>();
    }

    public record GetSalesByYear
    {
        public required Item[] Items { get; init; }

        public record Item
        {
            public required int Year { get; init; }
            public required decimal Amount { get; init; }
            public required Customer[] Customers { get; init; }

            public record Customer
            {
                public required decimal TotalAmount { get; init; }
                public required CustomerId CustomerId { get; init; }
            }
        }
    }
}
