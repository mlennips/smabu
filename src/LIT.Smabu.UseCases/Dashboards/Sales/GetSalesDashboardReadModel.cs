using LIT.Smabu.Domain.Common;

namespace LIT.Smabu.UseCases.Dashboards.Sales
{
    public record GetSalesDashboardReadModel
    {
        public DateTime Version { get; internal set; }
        public int ThisYear { get; internal set; }
        public int LastYear { get; internal set; }
        public Currency Currency { get; internal set; } = Currency.EUR;

        public decimal TotalSales { get; internal set; }
        public decimal SalesThisYear { get; internal set; }
        public decimal SalesLastYear { get; internal set; }
        public decimal SalesLast12Month { get; internal set; }
        public decimal SalesLast24Month { get; internal set; }
        public decimal SalesLast36Month { get; internal set; }

        public List<SalesAmountItem> Top3InvoicesEver { get; internal set; } = [];
        public List<SalesAmountItem> Top3InvoicesLast12Month { get; internal set; } = [];
        public List<SalesAmountItem> Top3CustomersEver { get; internal set; } = [];
        public List<SalesAmountItem> Top3CustomersLast12Month { get; internal set; } = [];

        public Dataset SalesByYear { get; internal set; } = new Dataset();
        public List<SalesAmountItem> SalesByCustomer { get; internal set; } = [];
        public int InvoiceCount { get; internal set; }
        public int CustomerCount { get; internal set; }
        public int OrderCount { get; internal set; }

        public record Dataset
        {
            public List<Serie> Series { get; internal set; } = [];
            public List<string> ValueLabels { get; internal set; } = [];

            public record Serie(string Label, string Key, string Group = "")
            {
                public List<decimal> Values { get; internal set; } = [];
            }
        }

        public record SalesAmountItem(string Name, string Key, decimal Total);

    }
}