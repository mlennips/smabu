using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Financial
{
    public record class AnnualFinancialStatementDTO : IDTO
    {
        public AnnualFinancialStatementId Id { get; init; }
        public string DisplayName { get; }
        public int FiscalYear { get; init; }
        public DatePeriod Period { get; init; }
        public Currency Currency { get; init; }
        public FinancialTransaction[] Incomes { get; init; }
        public FinancialTransaction[] Expenditures { get; init; }
        public FinancialStatementStatus Status { get; init; }
        public decimal TotalIncome { get; init; }
        public decimal TotalExpenditure { get; init; }
        public decimal NetIncome { get; init; }

        public AnnualFinancialStatementDTO(AnnualFinancialStatementId id, string displayName, int fiscalYear, DatePeriod period, Currency currency,
            FinancialTransaction[] incomes, FinancialTransaction[] expenditures, FinancialStatementStatus
            status, decimal totalIncome, decimal totalExpenditure, decimal netIncome)
        {
            Id = id;
            DisplayName = displayName;
            FiscalYear = fiscalYear;
            Period = period;
            Currency = currency;
            Incomes = incomes;
            Expenditures = expenditures;
            Status = status;
            TotalIncome = totalIncome;
            TotalExpenditure = totalExpenditure;
            NetIncome = netIncome;
        }

        internal static AnnualFinancialStatementDTO Create(AnnualFinancialStatement statement)
        {
            return new AnnualFinancialStatementDTO(statement.Id, statement.DisplayName, statement.FiscalYear, statement.Period, statement.Currency,
                [.. statement.Incomes], [.. statement.Expenditures], statement.Status, statement.TotalIncome, statement.TotalExpenditure, statement.NetIncome);
        }
    }
}
